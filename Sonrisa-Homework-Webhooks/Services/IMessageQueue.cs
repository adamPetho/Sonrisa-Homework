using Sonrisa_Homework_Webhooks.Models;

namespace Sonrisa_Homework_Webhooks.Services
{
    public interface IMessageQueue
    {
        ValueTask EnqueueAsync(Notification notification, CancellationToken cancellationToken);

        ValueTask<Notification> DequeueAsync(CancellationToken cancellationToken);
    }
}
