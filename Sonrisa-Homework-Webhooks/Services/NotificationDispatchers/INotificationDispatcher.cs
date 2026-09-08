using Sonrisa_Homework_Webhooks.Models;

namespace Sonrisa_Homework_Webhooks.Services.NotificationDispatchers
{
    public interface INotificationDispatcher
    {
        Task DispatchAsync(
            Notification notification,
            CancellationToken cancellationToken);
    }
}
