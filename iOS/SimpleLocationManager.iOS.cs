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

		public SimpleLocationManager ()
		{
			InitLocationManager ();
		}

		public void StartLocationUpdates (LocationAccuracy accuracy, double smallestDisplacementMeters,
		                                  TimeSpan? interval = null, TimeSpan? fastestInterval = null)
		{
			locationManager.DesiredAccuracy = CLLocationAccuracy [accuracy];
			locationManager.DistanceFilter = smallestDisplacementMeters;

			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				if (CLLocationManager.Status != CLAuthorizationStatus.AuthorizedWhenInUse)
					locationManager.RequestWhenInUseAuthorization ();
				if (CLLocationManager.Status != CLAuthorizationStatus.AuthorizedAlways)
					locationManager.RequestAlwaysAuthorization ();
			}
			Console.WriteLine ("[SimpleLocation: Authorization status = " + CLLocationManager.Status + "]");

			var locationServicesEnabled = CLLocationManager.LocationServicesEnabled;
			Console.WriteLine ("[SimpleLocation: Location services enabled: " + locationServicesEnabled + "]");
			locationManager.StartUpdatingLocation ();

			if (locationServicesEnabled &&
			    (CLLocationManager.Status == CLAuthorizationStatus.AuthorizedWhenInUse || CLLocationManager.Status != CLAuthorizationStatus.AuthorizedAlways))
				Console.WriteLine ("[SimpleLocation: Location updates started]");
		}

		public void StopLocationUpdates ()
		{
			locationManager.StopUpdatingLocation ();
			Console.WriteLine ("[SimpleLocation: Location updates stopped]");
		}

		void InitLocationManager ()
		{
			if (locationManager == null) {
				locationManager = new CLLocationManager ();
				locationManager.AuthorizationChanged += (sender, e) => {
					Console.WriteLine ("[SimpleLocation: AuthorizationChanged]");

					if (e.Status == CLAuthorizationStatus.AuthorizedAlways || e.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
						locationManager.StartUpdatingLocation ();
					else
						locationManager.StopUpdatingLocation ();
				};
				locationManager.LocationsUpdated += (sender, e) => {
					Console.WriteLine ("[SimpleLocation: LocationsUpdated]");

					var location = e.Locations.Last ();
					LastLocation = new Location (location.Coordinate.Latitude, location.Coordinate.Longitude);
					LocationUpdated ();
				};

				if (locationManager.Location != null)
					LastLocation = new Location (locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude);
			}
		}
	}
}

