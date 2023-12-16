using AspNetCoreHero.ToastNotification.Abstractions;
using StackExchange.Redis;
using System.Threading.Channels;
using YemeniDriver.Interfaces;

namespace YemeniDriver.Service
{
    public class NotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationService> _logger;
        private readonly Channel<Notification> _channel;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        public NotificationService(IServiceProvider serviceProvider)
        {
            _channel = Channel.CreateUnbounded<Notification>();
            _serviceProvider = serviceProvider;
        }



        public ValueTask PushAsync(Notification notification) => _channel
            .Writer.WriteAsync(notification);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
