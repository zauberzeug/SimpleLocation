using System;

namespace PerpetualEngine.Location
{
    public class Location
    {
        double latitude;
        double longitude;
        double direction;
        double speed;

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { 
            get{ return latitude; }
            set{ latitude = Math.Round(value, 6); }
        }

        public double Longitude { 
            get{ return longitude; }
            set{ longitude = Math.Round(value, 6); }
        }

        public double Direction { 
            get{ return direction; }
            set{ direction = Math.Round(value, 2); }
        }

        public double Speed { 
            get{ return speed; }
            set{ speed = Math.Round(value, 2); }
        }

        public override string ToString()
        {
            return string.Format("[SimpleLocation: Latitude={0}, Longitude={1}]", Latitude, Longitude);
        }
    }
}

