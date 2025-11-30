using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    public class GeoCoordinate
    {
        public double Latitude { get; }
        public double Longitude { get; }
        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public double GetDistanceTo(GeoCoordinate other)
        {
            // Haversine formula
            double R = 6371000; // meters
            double lat1 = Latitude * Math.PI / 180;
            double lat2 = other.Latitude * Math.PI / 180;
            double dLat = (other.Latitude - Latitude) * Math.PI / 180;
            double dLon = (other.Longitude - Longitude) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        public double GetBearingTo(GeoCoordinate other)
        {
            double lat1 = Latitude * Math.PI / 180;
            double lon1 = Longitude * Math.PI / 180;
            double lat2 = other.Latitude * Math.PI / 180;
            double lon2 = other.Longitude * Math.PI / 180;

            double dLon = lon2 - lon1;

            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);

            double brng = Math.Atan2(y, x);
            return (brng * 180 / Math.PI + 360) % 360;
        }
        public double GetDistanceFromSegment(GeoCoordinate start, GeoCoordinate end)
        {
            double d13 = start.GetDistanceTo(this);
            double theta13 = start.GetBearingTo(this) * Math.PI / 180;
            double theta12 = start.GetBearingTo(end) * Math.PI / 180;

            double dXt = Math.Asin(Math.Sin(d13 / 6371000) * Math.Sin(theta13 - theta12)) * 6371000;
            double dAt = Math.Acos(Math.Cos(d13 / 6371000) / Math.Cos(dXt / 6371000)) * 6371000;

            // Check if point is behind start
            // We can check if the angle difference is > 90 degrees
            double angleDiff = Math.Abs((theta13 - theta12 + 3 * Math.PI) % (2 * Math.PI) - Math.PI);
            if (angleDiff > Math.PI / 2)
            {
                return d13;
            }

            // Check if point is beyond end
            double totalDist = start.GetDistanceTo(end);
            if (dAt > totalDist)
            {
                return end.GetDistanceTo(this);
            }

            return Math.Abs(dXt);
        }
    }
}