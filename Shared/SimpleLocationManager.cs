using System;

namespace PerpetualEngine.Location
{
    public partial class SimpleLocationManager
    {
        public Location LastLocation { get; set; }

        public Action LocationUpdated = delegate {
        };

        public Action LocationUpdatesStarted = delegate {
        };

        public Action LocationUpdatesStopped = delegate {
        };
    }
}

