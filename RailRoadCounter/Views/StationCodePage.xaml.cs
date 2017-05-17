using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace RailRoadCounter
{
    public partial class 
        StationCodePage : ContentPage
    {
        public ObservableCollection<Station> Stations = new ObservableCollection<Station>();
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

                    var request = HttpConnector.CreateGetConnection(new Uri($"http://tarifgd.ru/tar_online2/getstan.php?cod={e.NewTextValue}&poiskvh=1&pp=*&view=xml"));

                    var response = await HttpConnector.Client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {

                        var xmlResponse = await response.Content.ReadAsStreamAsync();
                        var stationSerializer = new XmlSerializer(typeof(StationXml));

                        var stationsXml = (StationXml)stationSerializer.Deserialize(xmlResponse);

                        Stations.Clear();
                        if (stationsXml.Stations != null)
                        {
                            foreach (var station in stationsXml.Stations)
                            {
                                Stations.Add(station);
                            }
                        }
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

