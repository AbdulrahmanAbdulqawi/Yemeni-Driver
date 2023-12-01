using Microsoft.AspNetCore.SignalR;
using Yemeni_Driver.Models;

namespace Yemeni_Driver.Service
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendNotificationToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveNotification", message);
        }

        // New method to handle new ride request event
        public async Task SendNewRideRequestToGroup(string groupName, Request newRequest)
        {
            await Clients.Group(groupName).SendAsync("ReceiveNewRideRequest", newRequest);
        }
    }
}
