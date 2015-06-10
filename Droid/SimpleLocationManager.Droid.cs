using System;
using System.Collections.Generic;
using Android.App;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;

namespace PerpetualEngine.Location
{
    public partial class SimpleLocationManager
        : Java.Lang.Object, IGoogleApiClientConnectionCallbacks, IGoogleApiClientOnConnectionFailedListener, ILocationListener
    {
        static Activity context;
        IGoogleApiClient googleApiClient;
        bool resolvingError;

        double smallestDisplacementMeters;
        int accuracy;
        long interval;
        long fastestInterval;

        Dictionary<LocationAccuracy, int> LocationRequestAccuracy = new Dictionary<LocationAccuracy, int> {
            { LocationAccuracy.High, LocationRequest.PriorityHighAccuracy },
            { LocationAccuracy.Balanced, LocationRequest.PriorityBalancedPowerAccuracy },
            { LocationAccuracy.Low, LocationRequest.PriorityLowPower },
        };

        public SimpleLocationManager()
        {
            googleApiClient = new GoogleApiClientBuilder(context)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(LocationServices.Api)
                .Build();
        }

        public static void SetContext(Activity activity)
        {
            context = activity;
        }

        public void StartLocationUpdates(LocationAccuracy accuracy, double smallestDisplacementMeters,
                                         TimeSpan? interval = null, TimeSpan? fastestInterval = null)
        {
            this.smallestDisplacementMeters = smallestDisplacementMeters;
            this.accuracy = LocationRequestAccuracy[accuracy];
            this.interval = (long)(interval ?? TimeSpan.FromHours(1)).TotalMilliseconds;
            this.fastestInterval = (long)(fastestInterval ?? TimeSpan.FromMinutes(10)).TotalMilliseconds;

            googleApiClient.Connect();
        }

        public void StopLocationUpdates()
        {
            if (!googleApiClient.IsConnected)
                return;
            
            LocationServices.FusedLocationApi.RemoveLocationUpdates(googleApiClient, this);
            googleApiClient.Disconnect();
            Console.WriteLine("[SimpleLocation: Location updates stopped]");
        }

        public void OnConnected(Bundle connectionHint)
        {
            var location = LocationServices.FusedLocationApi.GetLastLocation(googleApiClient);
            if (location != null)
                LastLocation = new Location(location.Latitude, location.Longitude);

            LocationServices.FusedLocationApi.RequestLocationUpdates(googleApiClient, CreateLocationRequest(), this);
            Console.WriteLine("[SimpleLocation: Location updates started]");
        }

        public void OnConnectionSuspended(int cause)
        {
            Console.WriteLine("[SimpleLocation: Connection suspended. Cause: {0}]", cause);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            if (resolvingError)
                return; // Already attempting to resolve an error.

            if (result.HasResolution)
                try {
                    resolvingError = true;
                    result.StartResolutionForResult(context, 1001);
                } catch (Exception e) {
                    Console.WriteLine("[SimpleLocation: Connection failed. Error: {0}]", e.Message);
                    googleApiClient.Connect(); // There was an error with the resolution intent. Try again.
                }
            else {
                GooglePlayServicesUtil.GetErrorDialog(result.ErrorCode, context, 9000)?.Show();
                resolvingError = true;
            }
        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            if (location == null)
                return;
            
            LastLocation = new Location(location.Latitude, location.Longitude);
            LocationUpdated();
        }

        LocationRequest CreateLocationRequest()
        {
            var locationRequest = new LocationRequest();
            locationRequest.SetSmallestDisplacement((float)smallestDisplacementMeters);
            locationRequest.SetPriority(accuracy);
            locationRequest.SetInterval(interval);
            locationRequest.SetFastestInterval(fastestInterval);
            return locationRequest;
        }
    }
}
