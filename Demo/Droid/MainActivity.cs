using Android.App;
using Android.Content.PM;
using Android.OS;
using Demo;
using PerpetualEngine.Location;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Demo.Droid
{
    [Activity(
        Label = "SimpleLocation.Droid",
        Icon = "@drawable/icon",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        App app;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);

            SimpleLocationManager.SetContext(this);
            SimpleLocationManager.HideNeverButtonOnUseLocationDialog = true;

            app = new App();

            LoadApplication(app);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            app.SimpleLocationManager.HandleResolutionResultForLocationSettings(requestCode, resultCode);

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}

