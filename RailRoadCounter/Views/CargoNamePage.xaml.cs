using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace RailRoadCounter
{
	public partial class CargoNamePage : ContentPage
	{
		public ObservableCollection<Cargo> CargoList = new ObservableCollection<Cargo>();
		
		public CargoNamePage()
		{
			InitializeComponent();

			String icon = "IconCheck.png";
			BindingContext = this;

			CargoNamesList.ItemsSource = CargoList;


			ToolbarItems.Add(new ToolbarItem
			{
				Icon = icon,
				Command = new Command(() =>
				{
					Device.BeginInvokeOnMainThread(async () =>
											{
												await Navigation.PopModalAsync();
											});

				}),
			});

			CargoNamesList.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
			{
				App.Request.Cargo = (Cargo)e.SelectedItem;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync();
                });
            };

			Search.TextChanged += async (sender, e) =>
			{
				if (e.NewTextValue.Length != 0)
				{
					Loader.IsVisible = true;

					var request = HttpConnector.CreateGetConnection(new Uri($"http://tarifgd.ru/tar_online2/getgruz.php?buk={e.NewTextValue}&poiskvh=1&view=xml"));

					var response = await HttpConnector.Client.SendAsync(request);
					if (response.IsSuccessStatusCode)
					{

						var xmlResponse = await response.Content.ReadAsStreamAsync();
						var cargoSerializer = new XmlSerializer(typeof(CargoXml));

						var cargoXml = (CargoXml) cargoSerializer.Deserialize(xmlResponse);

						CargoList.Clear();
						if (cargoXml.CargoList != null)
						{
							foreach (var cargo in cargoXml.CargoList)
							{
								CargoList.Add(cargo);
							}
						}
					}

					Loader.IsVisible = false;
					CargoNamesList.IsVisible = CargoList.Count != 0;
				}
				else
				{
					CargoNamesList.IsVisible = false;
				}
			};
		}
	}
}
