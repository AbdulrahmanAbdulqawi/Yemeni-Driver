using Microsoft.AspNetCore.SignalR;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;

namespace YemeniDriver.Service
{
    public class NotificationHub : Hub
    {
        public async Task SendRequestNotification(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveRequestNotification", message);
        }
    }
}
