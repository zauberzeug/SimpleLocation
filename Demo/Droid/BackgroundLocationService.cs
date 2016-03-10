using Android.App;

namespace Demo.Droid
{
    [Service]
    public class BackgroundLocationService : Service
    {
        public static App App;

        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            // Do nothing
            return null;
        }

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            App.StartLocationUpdates();

            return StartCommandResult.RedeliverIntent;
        }

        public override void OnDestroy()
        {
            App.StopLocationUpdates();

            base.OnDestroy();
        }
    }
}

