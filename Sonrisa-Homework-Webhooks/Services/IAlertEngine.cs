using Sonrisa_Homework_Webhooks.Models.Events;

namespace Sonrisa_Homework_Webhooks.Services
{
    public interface IAlertEngine
    {
        Task ProcessEventAsync(IEvent @event, CancellationToken cancellationToken);
    }
}
