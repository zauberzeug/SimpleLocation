using System;

namespace PerpetualEngine.Location
{
    public partial class SimpleLocationManager
    {
        public void StartLocationUpdates(LocationAccuracy accuracy, double smallestDisplacementMeters,
                                         TimeSpan? interval = null, TimeSpan? fastestInterval = null)
        {
        }

        public void StopLocationUpdates()
        {
        }

        public void SimulateNewLocation(Location l)
        {
            LastLocation = l;
            LocationUpdated();
        }
    }
}
