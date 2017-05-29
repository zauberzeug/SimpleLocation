using System;
using PerpetualEngine.Location;
using Xamarin.Forms;

namespace Demo
{
    public class App : Application
    {
        const string hello = "Hello, SimpleLocation!";
        const string started = "Location updates started";
        const string stopped = "Location updates stopped";
        
        public SimpleLocationManager SimpleLocationManager;
        public Button startButton;
        public Button stopButton;

        Label helloLabel;
        Label infoLabel;
        Label latitudeLabel;
        Label longitudeLabel;
        Label directionLabel;
        Label speedLabel;

        public App()
        {
            InitViews();

            InitSimpleLocationManager();

            MainPage = new ContentPage {
                Content = new StackLayout {
                    Padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20),
                    Spacing = 20,
                    Children = {
                        helloLabel,
                        latitudeLabel,
                        longitudeLabel,
                        directionLabel,
                        speedLabel,
                        infoLabel,
                        new StackLayout {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.EndAndExpand,
                            Spacing = 20,
                            Children = {
                                startButton,
                                stopButton,
                            }
                        }
                    },
                },
            };

        }

        public void StartLocationUpdates()
        {
            SimpleLocationManager.StartLocationUpdates(
                LocationAccuracy.High, 0, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1));
        }

        public void StopLocationUpdates()
        {
            SimpleLocationManager.StopLocationUpdates();
        }

        void InitSimpleLocationManager()
        {
            SimpleLocationManager = new SimpleLocationManager();
            SimpleLocationManager.LocationUpdatesStarted += () => infoLabel.Text = started;
            SimpleLocationManager.LocationUpdatesStopped += () => infoLabel.Text = stopped;
            SimpleLocationManager.LocationUpdated += delegate {
                latitudeLabel.Text = string.Format("Latitude: {0}", SimpleLocationManager.LastLocation.Latitude);
                longitudeLabel.Text = string.Format("Longitude: {0}", SimpleLocationManager.LastLocation.Longitude);
                directionLabel.Text = string.Format("Direction: {0} deg", SimpleLocationManager.LastLocation.Direction);
                speedLabel.Text = string.Format("Speed: {0} m/s", SimpleLocationManager.LastLocation.Speed);

                Console.WriteLine(SimpleLocationManager.LastLocation);
            };
        }

        void InitViews()
        {
            helloLabel = new Label {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Text = hello,
            };

            latitudeLabel = new Label();

            longitudeLabel = new Label();

            directionLabel = new Label();

            speedLabel = new Label();

            infoLabel = new Label {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.EndAndExpand,
            };

            startButton = new Button {
                WidthRequest = 100,
                BackgroundColor = Color.Green,
                TextColor = GetButtonTextColor(),
                Text = "Start",
            };

            stopButton = new Button {
                WidthRequest = 100,
                BackgroundColor = Color.Red,
                TextColor = GetButtonTextColor(),
                Text = "Stop",
            };
        }

        Color GetButtonTextColor()
        {
            return Device.OnPlatform(Color.Black, Color.White, Color.White);
        }

    }
}

