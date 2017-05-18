using System;
using Xamarin.Forms;
using System.Xml.Serialization;
using RailRoadCounter.Models;

namespace RailRoadCounter
{
    public partial class StartPage : ContentPage
    {
        private static bool _isDeparture;

        public static bool IsDeparture { get => _isDeparture; set => _isDeparture = value; }

        public StartPage()
        {
            InitializeComponent();
            InitHandlers();
            BindingContext = App.Request;
        }



        public void InitHandlers()
        {
            DepStationName.Focused += (object sender, FocusEventArgs e) =>
            {
                IsDeparture = true;
                Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new StationNamePage()));
            };
            DepStationCode.Focused += (object sender, FocusEventArgs e) =>
            {
                IsDeparture = true;
                Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new StationCodePage()));
            };
            ArrStationName.Focused += (object sender, FocusEventArgs e) =>
            {
                IsDeparture = false;
                Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new StationNamePage()));
            };
            ArrStationCode.Focused += (object sender, FocusEventArgs e) =>
            {
                IsDeparture = false;
                Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new StationCodePage()));
            };
            CargoName.Focused += (sender, e) =>
            {
                Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CargoNamePage()));
            };
            CargoCode.Focused += (sender, e) =>
            {
                Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CargoCodePage()));
            };

            Calculate.Clicked += async delegate
            {
                if (App.Request.DepartureStation== null)
                {
                    Device.BeginInvokeOnMainThread(() => {
                        DisplayAlert("Ошибка", "Выберите станцию отправления", "OK");
                    });
                    return;
                }
                if (App.Request.ArrivalStation == null)
                {
                    Device.BeginInvokeOnMainThread(() => {
                        DisplayAlert("Ошибка", "Выберите станцию назначения", "OK");
                    });
                    return;
                }
                if (App.Request.Cargo == null)
                {
                    Device.BeginInvokeOnMainThread(() => {
                        DisplayAlert("Ошибка", "Выберите груз", "OK");
                    });
                    return;
                }
                var request = HttpConnector.CreateGetConnection(new Uri($"http://tarifgd.ru/tar_online2/prod2009/tar_raschet_only.php?tarID=280&st1={App.Request.DepartureStation.Code}&st2={App.Request.ArrivalStation.Code}&kgr={App.Request.Cargo.Code}&ves={App.Request.DepartureWeight}&nvohr={App.Request.NumOfGuardedWagons}&nprov={App.Request.NumOfConductors}&nv={App.Request.NumOfWagons}&osi={App.Request.NumOfAxis}&tip=3&st1_opt=1&st2_opt=1&period=last&sv=2&view=xml"));
                var response = await HttpConnector.Client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var xmlResponse = await response.Content.ReadAsStreamAsync();
                    var responseSerializer = new XmlSerializer(typeof(Response));

                    var responseXML = (Response)responseSerializer.Deserialize(xmlResponse);
                }

            };
        }
    }
}
