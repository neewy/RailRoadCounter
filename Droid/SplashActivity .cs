using Android.App;
using Android.Support.V7.App;

namespace RailRoadCounter.Droid
{
    [Activity(Label = "RailRoadCounter.Droid", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}