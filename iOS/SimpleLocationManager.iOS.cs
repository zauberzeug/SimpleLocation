using System;
using System.Collections.Generic;
using System.Linq;
using CoreLocation;
using UIKit;

namespace PerpetualEngine.Location
{
    public partial class SimpleLocationManager
    {
        CLLocationManager locationManager;

        Dictionary<LocationAccuracy, double> CLLocationAccuracy = new Dictionary<LocationAccuracy, double> {
            { LocationAccuracy.High, CLLocation.AccuracyBest },
            { LocationAccuracy.Balanced, CLLocation.AccuracyHundredMeters },
            { LocationAccuracy.Low, CLLocation.AccuracyKilometer },
        };

        public SimpleLocationManager()
        {
            InitLocationManager();
        }

        public void StartLocationUpdates(LocationAccuracy accuracy, double smallestDisplacementMeters,
                                         TimeSpan? interval = null, TimeSpan? fastestInterval = null)
        {
            if (!CLLocationManager.LocationServicesEnabled)
                return;
            
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
                locationManager.RequestWhenInUseAuthorization();
                locationManager.RequestAlwaysAuthorization();
            }

            locationManager.DesiredAccuracy = CLLocationAccuracy[accuracy];
            locationManager.DistanceFilter = smallestDisplacementMeters;

            locationManager.StartUpdatingLocation();
            Console.WriteLine("[SimpleLocation: Location updates started]");
        }

        public void StopLocationUpdates()
        {
            locationManager.StopUpdatingLocation();
            Console.WriteLine("[SimpleLocation: Location updates stopped]");
        }

        void InitLocationManager()
        {
            if (locationManager == null) {
                locationManager = new CLLocationManager();
                locationManager.LocationsUpdated += (sender, e) => {
                    var location = e.Locations.Last();
                    LastLocation = new Location(location.Coordinate.Latitude, location.Coordinate.Longitude);
                    LocationUpdated();
                };

                if (locationManager.Location != null)
                    LastLocation = new Location(locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude);
            }
        }
    }
}

