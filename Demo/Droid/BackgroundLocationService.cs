using System;
using Android.App;
using PerpetualEngine.Location;

namespace Demo.Droid
{
    [Service]
    public class BackgroundLocationService : Service
    {
        public static SimpleLocationManager SimpleLocationManager;

        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            // Do nothing
            return null;
        }

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            SimpleLocationManager.StartLocationUpdates(LocationAccuracy.High, 0, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

            return StartCommandResult.RedeliverIntent;
        }

        public override void OnDestroy()
        {
            SimpleLocationManager.StopLocationUpdates();

            base.OnDestroy();
        }
    }
}

