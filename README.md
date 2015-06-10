# SimpleLocation
Simple cross-platform location manager for *Xamarin* (*Android* and *iOS*)

## Features
- latest native location APIs accessible from shared code
- get last location
- receive location updates
- different accuracy levels (high, balanced, low)
- distance filter

## Setup
### Android
Add permission `ACCESS_FINE_LOCATION` to your `AndroidManifest.xml`:

    <uses-permission android:name=„android.permission.ACCESS_FINE_LOCATION“ />

Set a context (must be an `Activity`) for your `SimpleLocationManager`, f.e. in `OnCreate()` of your `MainActivity`: 

    SimpleLocationManager.SetContext(this);

### iOS
Add entries for `NSLocationAlwaysUsageDescription` and `NSLocationWhenInUseUsageDescription` to your `Info.plist`:

    <key>NSLocationAlwaysUsageDescription</key>
    <string>Can we always use your location</string>
    <key>NSLocationWhenInUseUsageDescription</key>
    <string>Can we use your location</string>

## Usage
Create an instance of `SimpleLocationManager`:

    var simpleLocMan = new SimpleLocationManager();

Define what should happen when the location gets updated:

    simpleLocMan.LocationUpdated += delegate {
        Console.WriteLine(simpleLocMan.LastLocation);
    }

To start location updates call

    simpleLocMan.StartLocationUpdates(LocationAccuracy.Balanced, 5, TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(30));

The `TimeSpan` parameters will be taken into consideration **only** on *Android* (see [here](https://developers.google.com/android/reference/com/google/android/gms/location/LocationRequest#setInterval(long)) for `interval` and [here](https://developers.google.com/android/reference/com/google/android/gms/location/LocationRequest#setFastestInterval(long)) for `fastestInterval`). On *iOS* just call

    simpleLocMan.StartLocationUpdates(LocationAccuracy.Balanced, 5);

Now every time the location gets successfully updated  `SimpleLocationManager.LastLocation` gets updated and `SimpleLocationManager.LocationUpdated` gets called.

To stop location updates just call

    simpleLocMan.StopLocationUpdates();
