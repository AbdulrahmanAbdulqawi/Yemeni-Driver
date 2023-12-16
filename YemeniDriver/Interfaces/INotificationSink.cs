using AspNetCoreHero.ToastNotification.Abstractions;

namespace YemeniDriver.Interfaces
{
    public interface INotificationSink
    {
        Task ReceiveNotification(string message);
    }
}
