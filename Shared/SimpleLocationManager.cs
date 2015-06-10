using System;

namespace PerpetualEngine.Location
{
    public partial class SimpleLocationManager
    {
        public Location LastLocation { get; set; }

        public event Action LocationUpdated = delegate { };
    }
}

