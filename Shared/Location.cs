namespace PerpetualEngine.Location
{
    public class Location
    {
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Direction { get; set; }

        public double Speed { get; set; }

        public override string ToString()
        {
            return string.Format("[SimpleLocation: Latitude={0}, Longitude={1}]", Latitude, Longitude);
        }
    }
}

