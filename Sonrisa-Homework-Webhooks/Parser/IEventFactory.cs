using Sonrisa_Homework_Webhooks.Models.Events;

namespace Sonrisa_Homework_Webhooks.Parser
{
    public interface IEventFactory
    {
        IEvent Create(EventRequest request);
    }
}
