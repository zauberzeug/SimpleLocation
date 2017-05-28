using Android.App;
using Android.Content.PM;
using Android.OS;
using Demo;
using PerpetualEngine.Location;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;

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
            SimpleLocationManager.HowOftenShowUseLocationDialog = SimpleLocationManager.ShowUseLocationDialog.Once;
            SimpleLocationManager.HandlePermissions = true;

            app = new App();
            BackgroundLocationService.App = app;

            app.startButton.Clicked += delegate {
                StartService(new Intent(this, typeof(BackgroundLocationService)));
            };
            app.stopButton.Clicked += delegate {
                StopService(new Intent(this, typeof(BackgroundLocationService)));
            };

            LoadApplication(app);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            app.SimpleLocationManager.HandleResolutionResultForLocationSettings(requestCode, resultCode);

            base.OnActivityResult(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            app.SimpleLocationManager.HandleResultForLocationPermissionRequest(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

