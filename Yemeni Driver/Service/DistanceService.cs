using GoogleMaps.LocationServices;

namespace Yemeni_Driver.Service
{
    public static class DistanceService
    {
        private const double EarthRadiusKm = 6371.0;
        private const double RatePerKilometer = 0.5; // Adjust this rate as needed


        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Convert latitude and longitude from degrees to radians
            lat1 = DegreesToRadians(lat1);
            lon1 = DegreesToRadians(lon1);
            lat2 = DegreesToRadians(lat2);
            lon2 = DegreesToRadians(lon2);

            // Calculate differences
            var dLat = lat2 - lat1;
            var dLon = lon2 - lon1;

            // Haversine formula
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Distance in kilometers
            var distance = EarthRadiusKm * c;

            return distance;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
        public static double EstimatePrice(string pickupLocation, string dropoffLocation)
        {
            var geoCodingService = new GeocodingService(Data.Constants.API_KEY);
            var pickupLoc = geoCodingService.GetCoordinatedFromAddress(pickupLocation);
            var dropoffLoc = geoCodingService.GetCoordinatedFromAddress(dropoffLocation);


            double distanceInKilometers = CalculateDistance(pickupLoc.Result.Latitude, pickupLoc.Result.Longitude,
                dropoffLoc.Result.Latitude, dropoffLoc.Result.Longitude);
            double estimatedPrice = distanceInKilometers * RatePerKilometer;

            return estimatedPrice;
        }
    }
}
