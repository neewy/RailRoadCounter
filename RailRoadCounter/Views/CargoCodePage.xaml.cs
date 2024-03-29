﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace RailRoadCounter
{
	public partial class CargoCodePage : ContentPage
	{
		public ObservableCollection<Cargo> CargoList = new ObservableCollection<Cargo>();
		private CargoService _cargoService;
        protected override bool OnBackButtonPressed()
        {
            StartPage.IsNavOpened = false;
            return base.OnBackButtonPressed();
        }
        public CargoCodePage()
		{
			InitializeComponent();

			String icon = "IconCheck.png";
			BindingContext = this;
			_cargoService = new CargoService(App.Database.sqlite);

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

					var databaseCargo = await _cargoService.FindByCode(e.NewTextValue);

					if (databaseCargo.Count == 0)
					{
						if (!DownloadCheker.IsCargoDownloadedByCode(e.NewTextValue))
						{
							DataRetrievalHelper dataHelper = new DataRetrievalHelper();
							await dataHelper.GetAndSaveCargoByCode(e.NewTextValue[0]);
							databaseCargo = await _cargoService.FindByCode(e.NewTextValue);
						}
					}

					CargoList.Clear();

					foreach (var cargo in databaseCargo)
					{
						CargoList.Add(cargo);
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
