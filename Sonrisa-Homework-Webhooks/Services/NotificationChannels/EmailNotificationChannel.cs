using Sonrisa_Homework_Webhooks.Models;

namespace Sonrisa_Homework_Webhooks.Services.NotificationChannels
{
    public sealed class EmailNotificationChannel : INotificationChannel
    {
        public string Name => "email";

        public Task SendAsync(Notification notification, CancellationToken cancellationToken)
        {
            //TODO: Proper Implementation
            Console.WriteLine(
                $"[EMAIL] {notification.Subject}: {notification.Message}");

            return Task.CompletedTask;
        }
    }
}
