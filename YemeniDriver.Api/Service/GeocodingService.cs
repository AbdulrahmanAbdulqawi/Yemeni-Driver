using GoogleMaps.LocationServices;
using YemeniDriver.Api.Interfaces;

namespace YemeniDriver.Api.Service
{
    public class GeocodingService : IGeocodingService
    {
        private readonly string apiKey; // Replace with your actual API key

        public GeocodingService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<AddressData> GetAddressFromCoordinates(double latitude, double longitude)
        {
            
            GoogleLocationService googleLocationService = new GoogleLocationService(apiKey);
            var address = googleLocationService.GetAddressFromLatLang(latitude, longitude);
            return address;
        }

        public async Task<MapPoint> GetCoordinatedFromAddress(string location)
        {
            GoogleLocationService googleLocationService = new GoogleLocationService(apiKey);
            var address = googleLocationService.GetLatLongFromAddress(location);
            return address;
        }
    }
}
