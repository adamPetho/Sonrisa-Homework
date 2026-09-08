using Sonrisa_Homework_Webhooks.Models;

namespace Sonrisa_Homework_Webhooks.Services.NotificationDispatchers
{
    public sealed class NotificationDispatcher : INotificationDispatcher
    {
        private readonly IReadOnlyDictionary<string, INotificationChannel> _channels;

        public NotificationDispatcher(
            IEnumerable<INotificationChannel> channels)
        {
            _channels = channels.ToDictionary(
                x => x.Name,
                StringComparer.OrdinalIgnoreCase);
        }

        public async Task DispatchAsync(Notification notification, CancellationToken cancellationToken)
        {
            if (!_channels.TryGetValue(notification.Channel, out var channel))
            {
                throw new InvalidOperationException(
                    $"Unknown notification channel: {notification.Channel}");
            }

            await channel.SendAsync(notification, cancellationToken);
        }
    }
}
