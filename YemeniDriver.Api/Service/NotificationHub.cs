using Microsoft.AspNetCore.SignalR;

namespace YemeniDriver.Api.Service
{
    public class NotificationHub : Hub
    {
        public async Task SendRequestNotification(string userId, string message, string driverId, string tripId)
        {
            await Clients.User(userId).SendAsync("ReceiveRequestNotification", message, driverId, tripId );
        }
    }
}
