using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace RailRoadCounter.Droid
{
    [Activity(Label = "RailRoadCounter.Droid", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}