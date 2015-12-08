using System;
using System.Collections.Generic;
using Android.App;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using Android.Content;

namespace PerpetualEngine.Location
{
    public partial class SimpleLocationManager : Java.Lang.Object, 
        IGoogleApiClientConnectionCallbacks, IGoogleApiClientOnConnectionFailedListener, ILocationListener, IResultCallback
    {
        static Activity context;
        IGoogleApiClient googleApiClient;
        bool resolvingError;
        const int requestResolveError = 1001;
        const int requestCheckSettings = 2002;

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
                .AddApi(LocationServices.API)
                .Build();
        }

        public static bool HideNeverButtonOnUseLocationDialog{ get; set; }

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
            LocationUpdatesStopped();
            SimpleLocationLogger.Log("Location updates stopped");
        }

        public void OnConnected(Bundle connectionHint)
        {
            CheckLocationServicesEnabled();
        }

        public void OnConnectionSuspended(int cause)
        {
            SimpleLocationLogger.Log("Connection suspended. Cause: " + cause);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            if (resolvingError)
                return; // Already attempting to resolve an error.

            if (result.HasResolution)
                try {
                    resolvingError = true;
                    result.StartResolutionForResult(context, requestResolveError);
                } catch (Exception e) {
                    SimpleLocationLogger.Log("Connection failed. Error: " + e.Message);
                    googleApiClient.Connect(); // There was an error with the resolution intent. Try again.
                }
            else {
                var dialog = GoogleApiAvailability.Instance.GetErrorDialog(context, result.ErrorCode, requestResolveError);
                dialog.DismissEvent += (sender, e) => resolvingError = false;
                dialog.Show();

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

        LocationSettingsRequest.Builder CreateLocationSettingsRequestBuilder()
        {
            var builder = new LocationSettingsRequest.Builder();
            builder.SetAlwaysShow(HideNeverButtonOnUseLocationDialog);
            builder.AddLocationRequest(CreateLocationRequest());
            return builder;
        }

        void CheckLocationServicesEnabled()
        {
            var result = LocationServices.SettingsApi.CheckLocationSettings(googleApiClient, CreateLocationSettingsRequestBuilder().Build());
            result.SetResultCallback(this);
        }

        public void OnResult(Java.Lang.Object result)
        {
            var locationSettingsResult = result as LocationSettingsResult;

            var status = locationSettingsResult.Status;
            switch (status.StatusCode) {
                case CommonStatusCodes.Success:
                    SimpleLocationLogger.Log("All location settings are satisfied");
                    StartUpdates();
                    break;
                case CommonStatusCodes.ResolutionRequired:
                    SimpleLocationLogger.Log("Location settings are not satisfied");
                    try {
                        status.StartResolutionForResult(context, requestCheckSettings);
                        // Handle result in OnActivityResult of your Activity by calling HandleResolutionResultForLocationSettings
                    } catch (IntentSender.SendIntentException) {
                        SimpleLocationLogger.Log("PendingIntent unable to execute request");
                    }
                    break;
                case LocationSettingsStatusCodes.SettingsChangeUnavailable:
                    SimpleLocationLogger.Log("Location settings are inadequate and cannot be fixed here");
                    break;
            }
        }

        public void HandleResolutionResultForLocationSettings(int requestCode, Result resultCode)
        {
            switch (requestCode) {
                case requestCheckSettings:
                    switch (resultCode) {
                        case Result.Ok:
                            SimpleLocationLogger.Log("User agreed to make required location settings changes");
                            StartUpdates();
                            break;
                        case Result.Canceled:
                            SimpleLocationLogger.Log("User chose not to make required location settings changes");
                            break;
                    }
                    break;
            }
        }

        void StartUpdates()
        {
            var location = LocationServices.FusedLocationApi.GetLastLocation(googleApiClient);
            if (location != null)
                LastLocation = new Location(location.Latitude, location.Longitude);

            LocationServices.FusedLocationApi.RequestLocationUpdates(googleApiClient, CreateLocationRequest(), this);
            SimpleLocationLogger.Log("Location updates started");
        }
    }
}
