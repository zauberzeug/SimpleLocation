using System;
using System.Windows.Input;
using PerpetualEngine.Location;
using Xamarin.Forms;

namespace Demo
{
    public class MainPageViewModel : BaseViewModel
    {
        Color _buttonColor = Color.Green;
        string _buttonText = "Start";
        SimpleLocationManager _locationManager;
        bool _locationUpdatesStarted;
        double _latitude;
        double _longitude;

        public MainPageViewModel(SimpleLocationManager locationManager)
        {
            InitLocationManager(locationManager);

            ToggleButtonCommand = new Command(ToggleLocationUpdates);
        }


        public ICommand ToggleButtonCommand { get; private set; }

        public double Latitude
        {
            get => _latitude;
            set => SetValue(ref _latitude, value);
        }

        public double Longitude
        {
            get => _longitude;
            set => SetValue(ref _longitude, value);
        }

        public Color ButtonColor
        {
            get => _buttonColor;
            set => SetValue(ref _buttonColor, value);
        }

        public string ButtonText
        {
            get => _buttonText;
            set => SetValue(ref _buttonText, value);
        }

        void InitLocationManager(SimpleLocationManager locationManager)
        {
            _locationManager = locationManager;
            _locationManager.LocationUpdatesStarted += OnLocationUpdatesStarted;
            _locationManager.LocationUpdatesStopped += OnLocationUpdatesStopped;
            _locationManager.LocationUpdated += OnLocationUpdated;
        }

        void OnLocationUpdatesStarted()
        {
            _locationUpdatesStarted = true;
            ButtonColor = Color.Red;
            ButtonText = "Stop";
        }

        void OnLocationUpdatesStopped()
        {
            _locationUpdatesStarted = false;
            ButtonColor = Color.Green;
            ButtonText = "Start";
        }

        void OnLocationUpdated()
        {
            var lastLocation = _locationManager.LastLocation;
            Latitude = lastLocation.Latitude;
            Longitude = lastLocation.Longitude;
            Console.WriteLine(lastLocation);
        }

        void ToggleLocationUpdates()
        {
            if (_locationUpdatesStarted)
                _locationManager.StopLocationUpdates();
            else
                _locationManager.StartLocationUpdates(LocationAccuracy.High, 0, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }
    }
}
