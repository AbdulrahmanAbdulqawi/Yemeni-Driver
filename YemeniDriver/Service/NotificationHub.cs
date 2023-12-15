using Microsoft.AspNetCore.SignalR;
using Yemeni_Driver.Interfaces;
using YemeniDriver.Models;

namespace YemeniDriver.Service
{
    public class NotificationHub : Hub
    {
        public async Task SendRequestNotification(string driverUserId, string message)
        {
            await Clients.User(driverUserId).SendAsync("ReceiveRequestNotification", message);
        }
    }
}
