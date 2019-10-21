using System;
using PerpetualEngine.Location;
using Xamarin.Forms;

namespace Demo
{
    public partial class App : Application
    {
        public SimpleLocationManager SimpleLocationManager { get; private set; }

        public App()
        {
            InitializeComponent();

            SimpleLocationManager = new SimpleLocationManager();
            var viewModel = new MainPageViewModel(SimpleLocationManager);
            MainPage = new MainPage(viewModel);
        }

        public void StartLocationUpdates()
        {
            SimpleLocationManager.StartLocationUpdates(LocationAccuracy.High, 0, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        public void StopLocationUpdates()
        {
            SimpleLocationManager.StopLocationUpdates();
        }
    }
}
