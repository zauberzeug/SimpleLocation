using System;
using PerpetualEngine.Location;
using Xamarin.Forms;

namespace Demo
{
    public class App : Application
    {
        public SimpleLocationManager SimpleLocationManager = new SimpleLocationManager();
        public Button startButton;
        public Button stopButton;

        const string hello = "Hello, SimpleLocation!";
        const string stopped = "Location updates stopped.";

        Label helloLabel;
        Label locationLabel;

        public App()
        {
            InitViews();

            MainPage = new ContentPage {
                Content = new StackLayout {
                    Padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20),
                    Spacing = 20,
                    Children = {
                        helloLabel,
                        locationLabel,
                        new StackLayout {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.End,
                            Spacing = 20,
                            Children = {
                                startButton,
                                stopButton,
                            }
                        }
                    },
                },
            };

            SimpleLocationManager.LocationUpdated += delegate {
                var locationDataString = string.Format(
                                             "New location:\nLat={0}\nLng={1}", 
                                             SimpleLocationManager.LastLocation.Latitude,
                                             SimpleLocationManager.LastLocation.Longitude);
                locationLabel.Text = locationDataString;
                Console.WriteLine(locationDataString);
            };
            SimpleLocationManager.LocationUpdatesStopped += () => locationLabel.Text = stopped;
        }

        public void StartLocationUpdates()
        {
            SimpleLocationManager.StartLocationUpdates(
                LocationAccuracy.High, 1, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1));
        }

        public void StopLocationUpdates()
        {
            SimpleLocationManager.StopLocationUpdates();
        }

        void InitViews()
        {
            helloLabel = new Label {
                VerticalOptions = LayoutOptions.Start,
                XAlign = TextAlignment.Center,
                Text = hello,
            };

            locationLabel = new Label {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                XAlign = TextAlignment.Center,
            };

            startButton = new Button {
                WidthRequest = 100,
                BackgroundColor = Color.Green,
                TextColor = Device.OnPlatform(Color.Black, Color.White, Color.White),
                Text = "Start",
            };

            stopButton = new Button {
                WidthRequest = 100,
                BackgroundColor = Color.Red,
                TextColor = Device.OnPlatform(Color.Black, Color.White, Color.White),
                Text = "Stop",
            };
        }

    }
}

