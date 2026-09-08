using Sonrisa_Homework_Webhooks.Models;

namespace Sonrisa_Homework_Webhooks.Services
{
    public interface INotificationChannel
    {
        string Name { get; }
        Task SendAsync(Notification notification, CancellationToken cancellationToken);
    }
}
