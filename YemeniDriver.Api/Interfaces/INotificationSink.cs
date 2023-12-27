namespace YemeniDriver.Api.Interfaces
{
    public interface INotificationSink
    {
        Task ReceiveNotification(string message);
    }
}
