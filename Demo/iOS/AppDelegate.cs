using System;
using System.IO;
using Foundation;
using PerpetualEngine.Location;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Demo.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        string locationsLog = "";

        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Forms.Init();

            SimpleLocationManager.RequestAlwaysAuthorization = true;

            var app = new App();
            app.SimpleLocationManager.LocationUpdated += delegate {
                var locationDataString = string.Format(
                                             "New location:\nLat={0}\nLng={1}", 
                                             app.SimpleLocationManager.LastLocation.Latitude,
                                             app.SimpleLocationManager.LastLocation.Longitude);
                locationsLog += string.Format("{0}\n{1}\n\n", DateTime.Now, locationDataString);
            };
            app.SimpleLocationManager.LocationUpdatesStopped += delegate {
                LogLocationUpdates();
            };

            LoadApplication(app);

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        void LogLocationUpdates()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, "LocationsLog.txt");
            File.WriteAllText(filename, locationsLog);
        }
    }
}

