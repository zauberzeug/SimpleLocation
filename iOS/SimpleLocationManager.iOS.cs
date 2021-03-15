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
        bool isInitializing = true;
        bool shouldBeUpdatingLocation;

        Dictionary<LocationAccuracy, double> CLLocationAccuracy = new Dictionary<LocationAccuracy, double> {
            { LocationAccuracy.Navigation, CLLocation.AccurracyBestForNavigation },
            { LocationAccuracy.High, CLLocation.AccuracyBest },
            { LocationAccuracy.Balanced, CLLocation.AccuracyHundredMeters },
            { LocationAccuracy.Low, CLLocation.AccuracyKilometer },
        };

        public SimpleLocationManager()
        {
            InitLocationManager();
        }

        public event Action<object, CLAuthorizationChangedEventArgs> AuthorizationChanged = delegate { };

        public static bool RequestAlwaysAuthorization { get; set; } = false;

        public static bool PausesLocationUpdatesAutomatically { get; set; } = false;

        public void StartLocationUpdates(LocationAccuracy accuracy, double smallestDisplacementMeters,
                                         TimeSpan? interval = null, TimeSpan? fastestInterval = null)
        {
            shouldBeUpdatingLocation = true;
            locationManager.DesiredAccuracy = CLLocationAccuracy[accuracy];
            locationManager.DistanceFilter = smallestDisplacementMeters;
            locationManager.PausesLocationUpdatesAutomatically = PausesLocationUpdatesAutomatically;

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0) && RequestAlwaysAuthorization)
                locationManager.AllowsBackgroundLocationUpdates = true;

            TryToStartUpdates();
        }

        public void StopLocationUpdates()
        {
            shouldBeUpdatingLocation = false;
            locationManager.StopUpdatingLocation();
            LocationUpdatesStopped();
        }

        void InitLocationManager()
        {
            if (locationManager == null) {
                locationManager = new CLLocationManager();
                if (UIDevice.CurrentDevice.CheckSystemVersion(14, 0))
                    locationManager.DidChangeAuthorization += (sender, e) => OnAuthorizationChanged(sender, new CLAuthorizationChangedEventArgs(locationManager.AuthorizationStatus));
                else
                    locationManager.AuthorizationChanged += OnAuthorizationChanged;

                locationManager.LocationsUpdated += (sender, e) => {
                    var location = e.Locations.Last();
                    LastLocation = new Location(location.Coordinate.Latitude, location.Coordinate.Longitude);
                    LastLocation.Direction = location.Course >= 0 ? location.Course : 0;
                    LastLocation.Speed = location.Speed >= 0 ? location.Speed : 0;
                    if (location.VerticalAccuracy < 0 || location.HorizontalAccuracy < 0) // accurary can be invalid
                        LastLocation.Accuracy = -1;
                    else
                        LastLocation.Accuracy = Math.Max(location.VerticalAccuracy, location.HorizontalAccuracy);
                    LocationUpdated();
                };

                if (locationManager.Location != null)
                    LastLocation = new Location(locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude);
            }
        }

        void OnAuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
        {
            AuthorizationChanged(sender, e);
            if (isInitializing) { // Necessary because AuthorizationChanged get's called even on App start
                isInitializing = false;
                return;
            }

            SimpleLocationLogger.Log("Authorization changed to " + e.Status);

            if (AppHasLocationPermission()) {
                if (shouldBeUpdatingLocation)
                    TryToStartUpdates();
            } else {
                StopLocationUpdates();
                TriggerAppPermissionDialog();
            }
        }

        void TryToStartUpdates()
        {
            if (GlobalLocationServicesEnabled()) {
                if (AppHasLocationPermission()) {
                    locationManager.StartUpdatingLocation();
                    LocationUpdatesStarted();
                } else {
                    TriggerAppPermissionDialog();
                }
            } else {
                TriggerGlobalPermissionDialog();
            }
        }

        bool AppHasLocationPermission()
        {
            return UIDevice.CurrentDevice.CheckSystemVersion(8, 0) && IsAuthorized();
        }

        bool IsAuthorized()
        {
            var status = CLLocationManager.Status;
            SimpleLocationLogger.Log("Authorization status = " + status);
            return status == CLAuthorizationStatus.AuthorizedAlways || status == CLAuthorizationStatus.AuthorizedWhenInUse;
        }

        bool GlobalLocationServicesEnabled()
        {
            var locationServicesEnabled = CLLocationManager.LocationServicesEnabled;
            SimpleLocationLogger.Log("Location services enabled = " + locationServicesEnabled);
            return locationServicesEnabled;
        }

        void TriggerGlobalPermissionDialog()
        {
            locationManager.StartUpdatingLocation(); // HACK: Triggers system dialog to ask user to enable location services
        }

        void TriggerAppPermissionDialog()
        {
            if (RequestAlwaysAuthorization)
                locationManager.RequestAlwaysAuthorization();
            else
                locationManager.RequestWhenInUseAuthorization();
        }
    }
}

