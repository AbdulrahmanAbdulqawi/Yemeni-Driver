using GoogleMaps.LocationServices;

namespace YemeniDriver.Api.Interfaces
{
    public interface IGeocodingService
    {
        public Task<AddressData> GetAddressFromCoordinates(double latitude, double longitude);
        public Task<MapPoint> GetCoordinatedFromAddress(string location);


    }
}
