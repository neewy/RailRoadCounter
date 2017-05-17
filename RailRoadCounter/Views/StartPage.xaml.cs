using Xamarin.Forms;

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

		}
	}
}
