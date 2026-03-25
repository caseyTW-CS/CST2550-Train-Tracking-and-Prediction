using System;

namespace TrainProject
{
    public struct GeoCoords
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GeoCoords(double newLatitude, double newLongitude)
        {
            Latitude = newLatitude;
            Longitude = newLongitude;
        }

        // Haversine formula - calculates straight-line distance between
        // two sets of coordinates on the Earth's surface, returns miles
        public double distanceTo(GeoCoords other)
        {
            const double EarthRadiusMiles = 3958.8;

            double lat1 = toRadians(this.Latitude);
            double lat2 = toRadians(other.Latitude);
            double deltaLat = toRadians(other.Latitude - this.Latitude);
            double deltaLon = toRadians(other.Longitude - this.Longitude);

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusMiles * c;
        }

        // helper to convert degrees to radians
        private double toRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}