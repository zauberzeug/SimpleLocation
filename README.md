# SimpleLocation
Simple cross-platform location manager for *Xamarin* (*Android* and *iOS*)

## Features
- latest native location APIs accessible from shared code
- get last location
- receive location updates
- different accuracy levels (high, balanced, low)
- distance filter
- automatic error handling for location settings
- can be used in the background (f.e. in a `Service` on *Android*)

## Setup
### Android
Add permission `ACCESS_FINE_LOCATION` to your `AndroidManifest.xml`:

    <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />

Set a context for your `SimpleLocationManager`, f.e. in `OnCreate()` of your `MainActivity`:

    SimpleLocationManager.SetContext(this);

In case the device's location settings are disabled, the user gets informed by a system dialog (only available when your context is an `Activity`). If the user enables location settings this way and you want to automatically get location updates, you must call `HandleResolutionResultForLocationSettings` in your `Activity`'s `OnActivityResult` and pass the request code and result code.

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        simpleLocationManager.HandleResolutionResultForLocationSettings(requestCode, resultCode);
        ...  
    }

**Using background location:** Works out of the box now. Your context no longer needs to be an `Activity`. You can use SimpleLocationManager in a `Service` now.

### iOS
**Important:** Currently SimpleLocation only works on iOS 8 or higher.

Add entries for `NSLocationAlwaysUsageDescription` and `NSLocationWhenInUseUsageDescription` to your `Info.plist`:

    <key>NSLocationAlwaysUsageDescription</key>
    <string>We want to use your location when the app is in background</string>
    <key>NSLocationWhenInUseUsageDescription</key>
    <string>We want to use your location when the app is in foreground</string>

**Using background location:** If your app is supposed to get location updates while it is in the background, you have to do some additional setup.

Enable background mode and add location in your `Info.plist`:

	<key>UIBackgroundModes</key>
	<array>
		<string>location</string>
	</array>

Enable requesting authorization for usage in background, f.e. in your `AppDelegate` before starting location updates:

	SimpleLocationManager.RequestAlwaysAuthorization = true;

*Optionally* you can let iOS pause location updates automatically by setting

	SimpleLocationManager.PausesLocationUpdatesAutomatically = true;

On some devices this boolean is set to `true` by default, but the default value for `SimpleLocationManager` is `false`.

## Usage
Create an instance of `SimpleLocationManager`:

    var simpleLocationManager = new SimpleLocationManager();

Define what should happen when the location gets updated:

    simpleLocationManager.LocationUpdated += delegate {
        Console.WriteLine(simpleLocationManager.LastLocation);
    }

To start location updates call

    simpleLocationManager.StartLocationUpdates(LocationAccuracy.Balanced, 5, TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(30));

The `TimeSpan` parameters will be taken into consideration **only** on *Android* (see [here](https://developers.google.com/android/reference/com/google/android/gms/location/LocationRequest#setInterval(long)) for `interval` and [here](https://developers.google.com/android/reference/com/google/android/gms/location/LocationRequest#setFastestInterval(long)) for `fastestInterval`). On *iOS* just call

    simpleLocationManager.StartLocationUpdates(LocationAccuracy.Balanced, 5);

Now every time the location gets successfully updated  `SimpleLocationManager.LastLocation` gets updated and `SimpleLocationManager.LocationUpdated` gets called.

To stop location updates just call

    simpleLocationManager.StopLocationUpdates();

## Logging

SimpleLocation internally uses its own logger, a static class called `SimpleLocationLogger`. Its log statements appear like `[SimpleLocation: some message]`. You can disable logging by calling

    SimpleLocationLogger.Enabled = false;
