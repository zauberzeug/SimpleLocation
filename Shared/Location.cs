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

        /// <summary>
        /// Gets or sets the latitude in degrees (6 decimals).
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { 
            get{ return latitude; }
            set{ latitude = Math.Round(value, 6); }
        }

        /// <summary>
        /// Gets or sets the longitude in degrees (6 decimals).
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { 
            get{ return longitude; }
            set{ longitude = Math.Round(value, 6); }
        }

        /// <summary>
        /// Gets or sets the direction in which the device is moving horizontally. Measured in degrees East of true North (2 decimals).
        /// </summary>
        /// <value>The direction.</value>
        public double Direction { 
            get{ return direction; }
            set{ direction = Math.Round(value, 2); }
        }

        /// <summary>
        /// Gets or sets the speed of the device. Measured in meters per second (2 decimals)
        /// </summary>
        /// <value>The speed.</value>
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

