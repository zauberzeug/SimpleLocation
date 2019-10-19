using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PerpetualEngine.Location;
using Xamarin.Forms;

namespace Demo
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        Color _buttonColor = Color.Green;
        string _buttonText = "Start";

        public MainPageViewModel(SimpleLocationManager locationManager)
        {
            ToggleButtonCommand = new Command(() => ToggleButton());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ToggleButtonCommand { get; private set; }

        public Color ButtonColor
        {
            get => _buttonColor;
            set
            {
                if (_buttonColor == value)
                    return;

                _buttonColor = value;
                OnPropertyChanged();
            }
        }

        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if (_buttonText == value)
                    return;

                _buttonText = value;
                OnPropertyChanged();
            }
        }

        void ToggleButton()
        {
            ToggleButtonColor();
            ToggleButtonText();
        }

        void ToggleButtonColor() => ButtonColor = ButtonColor == Color.Green ? Color.Red : Color.Green;

        void ToggleButtonText() => ButtonText = ButtonText == "Start" ? "Stop" : "Start";

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
