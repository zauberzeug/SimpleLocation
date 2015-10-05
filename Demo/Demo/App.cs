using System;
using PerpetualEngine.Location;
using Xamarin.Forms;

namespace Demo
{
	public class App : Application
	{
		public SimpleLocationManager SimpleLocationManager = new SimpleLocationManager ();

		string hello = "Hello, SimpleLocation!";

		public App ()
		{
			var label = new Label {
				XAlign = TextAlignment.Center,
				Text = hello,
			};

			var startButton = new Button {
				Text = "Start location updates",
				Command = new Command (o => SimpleLocationManager.StartLocationUpdates (
					LocationAccuracy.High, 1, TimeSpan.FromSeconds (5), TimeSpan.FromSeconds (1))),
			};

			var stopButton = new Button {
				Text = "Stop location updates",
				Command = new Command (o => SimpleLocationManager.StopLocationUpdates ()),
			};

			MainPage = new ContentPage {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						label,
						startButton,
						stopButton,
					},
				},
			};

			SimpleLocationManager.LocationUpdated += () => label.Text = "New location:\n" + SimpleLocationManager.LastLocation;
			SimpleLocationManager.LocationUpdatesStopped += () => label.Text = hello;
		}

		protected override void OnSleep ()
		{
			SimpleLocationManager.StopLocationUpdates ();
		}
	}
}

