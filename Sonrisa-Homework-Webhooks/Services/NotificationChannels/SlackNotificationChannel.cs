using Sonrisa_Homework_Webhooks.Models;

namespace Sonrisa_Homework_Webhooks.Services.NotificationChannels
{
    public sealed class SlackNotificationChannel : INotificationChannel
    {
        public string Name => "slack";

        public Task SendAsync(Notification notification, CancellationToken cancellationToken)
        {
            //TODO: Proper implementation.
            Console.WriteLine(
                $"[SLACK] {notification.Subject}: {notification.Message}");

            return Task.CompletedTask;
        }
    }
}
