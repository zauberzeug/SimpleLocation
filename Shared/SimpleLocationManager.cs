using System;

namespace PerpetualEngine.Location
{
    public partial class SimpleLocationManager
    {
        public Location LastLocation { get; set; }

        public Action LocationUpdated = delegate {
            SimpleLocationLogger.Log("Location updated");
        };

        public Action LocationUpdatesStarted = delegate {
            SimpleLocationLogger.Log("Location updates started");
        };

        public Action LocationUpdatesStopped = delegate {
            SimpleLocationLogger.Log("Location updates stopped");
        };
    }
}

