using Microsoft.AspNetCore.SignalR;
using YemeniDriver.Models;

namespace YemeniDriver.Service
{
    public class RideHub : Hub
    {
        public async Task SendRideRequestUpdate(string message)
        {
            // Broadcast ride request updates to all connected clients
            await Clients.All.SendAsync("ReceiveRideRequestUpdate", message);
        }

        public async Task SendDriverLocationUpdate(string driverId, double latitude, double longitude)
        {
            // Broadcast driver location updates to all connected clients
            await Clients.All.SendAsync("ReceiveDriverLocationUpdate", driverId, latitude, longitude);
        }
    }
}
