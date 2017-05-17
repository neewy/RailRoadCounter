using Xamarin.Forms;

namespace RailRoadCounter
{
	public partial class App : Application
	{

		public static RequestData Request = new RequestData();

		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new StartPage());
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
