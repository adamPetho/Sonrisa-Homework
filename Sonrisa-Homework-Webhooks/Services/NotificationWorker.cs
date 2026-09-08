using Sonrisa_Homework_Webhooks.Services.NotificationDispatchers;

namespace Sonrisa_Homework_Webhooks.Services
{
    public sealed class NotificationWorker : BackgroundService
    {
        private readonly IMessageQueue _messageQueue;
        private readonly INotificationDispatcher _dispatcher;
        private readonly ILogger<NotificationWorker> _logger;

        public NotificationWorker(
            IMessageQueue messageQueue,
            INotificationDispatcher dispatcher,
            ILogger<NotificationWorker> logger)
        {
            _messageQueue = messageQueue;
            _dispatcher = dispatcher;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var notification = await _messageQueue
                        .DequeueAsync(stoppingToken);

                    await _dispatcher.DispatchAsync(notification, stoppingToken);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to process notification.");
                }
            }
        }
    }
}
