using Microsoft.AspNetCore.SignalR;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;

namespace YemeniDriver.Service
{
    public class NotificationHub : Hub
    {
        public async Task SendRequestNotification(string userId, string message, string driverId, string tripId)
        {
            await Clients.User(userId).SendAsync("ReceiveRequestNotification", message, driverId, tripId );
        }
    }
}
