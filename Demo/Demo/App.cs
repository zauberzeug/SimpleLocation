using System;
using PerpetualEngine.Location;
using Xamarin.Forms;

namespace Demo
{
    public class App : Application
    {
        public SimpleLocationManager SimpleLocationManager;

        public App()
        {
            InitSimpleLocationManager();

            MainPage = new MainPage();
        }

        public void StartLocationUpdates()
        {
            SimpleLocationManager.StartLocationUpdates(LocationAccuracy.High, 0, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        public void StopLocationUpdates()
        {
            SimpleLocationManager.StopLocationUpdates();
        }

        void InitSimpleLocationManager()
        {
            SimpleLocationManager = new SimpleLocationManager();
            SimpleLocationManager.LocationUpdatesStarted += () => { };
            SimpleLocationManager.LocationUpdatesStopped += () => { };
            SimpleLocationManager.LocationUpdated += () => Console.WriteLine(SimpleLocationManager.LastLocation);
        }
    }
}

