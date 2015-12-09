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
            locationManager.DesiredAccuracy = CLLocationAccuracy[accuracy];
            locationManager.DistanceFilter = smallestDisplacementMeters;

            if (GlobalLocationServicesEnabled()) {
                if (AppHasLocationPermission())
                    StartUpdates();
                else
                    TriggerAppPermissionDialog();					
            } else {
                TriggerGlobalPermissionDialog();
            }
        }

        public void StopLocationUpdates()
        {
            locationManager.StopUpdatingLocation();
            LocationUpdatesStopped();
        }

        void InitLocationManager()
        {
            if (locationManager == null) {
                locationManager = new CLLocationManager();
                locationManager.AuthorizationChanged += (sender, e) => {
                    SimpleLocationLogger.Log("AuthorizationChanged to " + e.Status);

                    if (AppHasLocationPermission())
                        StartUpdates();
                    else {
                        StopLocationUpdates();
                        TriggerAppPermissionDialog();
                    }
					
                };
                locationManager.LocationsUpdated += (sender, e) => {

                    var location = e.Locations.Last();
                    LastLocation = new Location(location.Coordinate.Latitude, location.Coordinate.Longitude);
                    LocationUpdated();
                };

                if (locationManager.Location != null)
                    LastLocation = new Location(locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude);
            }
        }

        void StartUpdates()
        {
            var locationServicesEnabled = GlobalLocationServicesEnabled();
            locationManager.StartUpdatingLocation();

            if (locationServicesEnabled && AppHasLocationPermission()) {
                LocationUpdatesStarted();
            }
        }

        bool AppHasLocationPermission()
        {
            return UIDevice.CurrentDevice.CheckSystemVersion(8, 0) && IsAuthorized(CLLocationManager.Status);
        }

        bool GlobalLocationServicesEnabled()
        {
            var locationServicesEnabled = CLLocationManager.LocationServicesEnabled;
            SimpleLocationLogger.Log("Location services enabled = " + locationServicesEnabled);
            return locationServicesEnabled;
        }

        void TriggerGlobalPermissionDialog()
        {
            if (!GlobalLocationServicesEnabled())
                locationManager.StartUpdatingLocation(); // HACK: Triggers system dialog to ask user to enable location services
        }

        void TriggerAppPermissionDialog()
        {
            if (!AppHasLocationPermission())
                locationManager.RequestWhenInUseAuthorization();
        }

        bool IsAuthorized(CLAuthorizationStatus status)
        {
            SimpleLocationLogger.Log("Authorization status = " + status);

            return status == CLAuthorizationStatus.AuthorizedAlways || status == CLAuthorizationStatus.AuthorizedWhenInUse;
        }
    }
}

