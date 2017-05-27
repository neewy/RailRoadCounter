using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PCLStorage;
using Xamarin.Forms;

namespace RailRoadCounter
{
	public partial class App : Application
	{

		private static Database _database;
		public static Database Database
		{
			get
			{
				if (_database == null || _database.sqlite == null)
				{
					_database = new Database(DependencyService.Get<IFileHelper>().GetLocalFilePath("Railroad.db3"));
				}
				return _database;
			}

			set
			{
				_database = value;
			}
		}

		public static RequestData Request = new RequestData();

		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new StartPage());
		}

		protected override void OnStart()
		{
			if (!Application.Current.Properties.ContainsKey("firstRun"))
			{
				App.Database.CreateInitialDatabase().ConfigureAwait(false);
				Application.Current.Properties.Add("firstRun", 1);
				Application.Current.SavePropertiesAsync().ConfigureAwait(false);
			}
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
