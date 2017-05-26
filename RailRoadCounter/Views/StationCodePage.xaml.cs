using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace RailRoadCounter
{
	public partial class StationCodePage : ContentPage
	{
		public ObservableCollection<Station> Stations = new ObservableCollection<Station>();
		private StationService _stationService;

		protected override void OnAppearing()
		{
			base.OnAppearing();
			Search.Focus(); //select when apprears
		}
		public StationCodePage()
		{
			InitializeComponent();

			String icon = "IconCheck.png";
			BindingContext = this;
			if (StartPage.IsDeparture)
			{
				Title = "Выбор станции отправления";
			}
			else
			{
				Title = "Выбор станции назначения";
			}
			_stationService = new StationService(App.Database.sqlite);

			StationNamesList.ItemsSource = Stations;

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

			StationNamesList.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
			{
				if (StartPage.IsDeparture)
				{
					App.Request.DepartureStation = (Station)e.SelectedItem;
				}
				else
				{
					App.Request.ArrivalStation = (Station)e.SelectedItem;
				}

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

					var databaseStations = await _stationService.FindByCode(e.NewTextValue);

					if (databaseStations.Count == 0)
					{
						if (!DownloadCheker.IsStationDownloadedByCode(e.NewTextValue))
						{
							DataRetrievalHelper dataHelper = new DataRetrievalHelper();
							await dataHelper.GetAndSaveStationsByCode(e.NewTextValue[0]);
							databaseStations = await _stationService.FindByCode(e.NewTextValue);
						}
					}

					Stations.Clear();

					foreach (var cargo in databaseStations)
					{
						Stations.Add(cargo);
					}

					Loader.IsVisible = false;
					StationNamesList.IsVisible = Stations.Count != 0;
				}
				else
				{
					StationNamesList.IsVisible = false;
				}
			};


		}
	}

}

