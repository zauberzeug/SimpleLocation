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

			CheckAppPermissions();
		}

		public void StopLocationUpdates()
		{
			locationManager.StopUpdatingLocation();
			LocationUpdatesStopped();
			Console.WriteLine("[SimpleLocation: Location updates stopped]");
		}

		void InitLocationManager()
		{
			if (locationManager == null) {
				locationManager = new CLLocationManager();
				locationManager.AuthorizationChanged += (sender, e) => {
					Console.WriteLine("[SimpleLocation: AuthorizationChanged to " + e.Status + "]");

					if (e.Status == CLAuthorizationStatus.AuthorizedAlways) {
						StartUpdates();
					} else
						StopLocationUpdates();
				};
				locationManager.LocationsUpdated += (sender, e) => {
					Console.WriteLine("[SimpleLocation: LocationsUpdated]");

					var location = e.Locations.Last();
					LastLocation = new Location(location.Coordinate.Latitude, location.Coordinate.Longitude);
					LocationUpdated();
				};

				if (locationManager.Location != null)
					LastLocation = new Location(locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude);
			}
		}

		void CheckAppPermissions()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
				Console.WriteLine("[SimpleLocation: Authorization status = " + CLLocationManager.Status + "]");

				if (CLLocationManager.Status != CLAuthorizationStatus.AuthorizedAlways) {
					if (!CLLocationManager.LocationServicesEnabled)
						locationManager.StartUpdatingLocation(); // HACK: Triggers system dialog to ask user to enable location services
					
					locationManager.RequestAlwaysAuthorization();
				} else {
					StartUpdates();
				}
			}
		}

		void StartUpdates()
		{
			var locationServicesEnabled = CLLocationManager.LocationServicesEnabled;
			Console.WriteLine("[SimpleLocation: Location services enabled: " + locationServicesEnabled + "]");
			locationManager.StartUpdatingLocation();

			if (locationServicesEnabled && CLLocationManager.Status == CLAuthorizationStatus.AuthorizedAlways) {
				LocationUpdatesStarted();
				Console.WriteLine("[SimpleLocation: Location updates started]");
			}
		}
	}
}

