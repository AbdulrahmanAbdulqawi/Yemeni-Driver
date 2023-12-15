using AspNetCoreHero.ToastNotification.Abstractions;

namespace Yemeni_Driver.Interfaces
{
    public interface INotificationSink
    {
        Task ReceiveNotification(string message);
    }
}
