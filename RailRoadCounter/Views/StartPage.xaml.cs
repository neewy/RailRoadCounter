using Xamarin.Forms;

namespace RailRoadCounter
{
	public partial class StartPage : ContentPage
	{
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
				Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new DepStationNamePage()));
			};

			CargoName.Focused += (sender, e) => 
			{ 
				Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CargoNamePage()));
			};

		}
	}
}
