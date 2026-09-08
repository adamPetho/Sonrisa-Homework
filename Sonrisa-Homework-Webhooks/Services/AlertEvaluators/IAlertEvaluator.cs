using Sonrisa_Homework_Webhooks.Models.Alerts;
using Sonrisa_Homework_Webhooks.Models.Events;

namespace Sonrisa_Homework_Webhooks.Services.AlertEvaluators
{
    public interface IAlertEvaluator
    {
        bool CanEvaluate(IEvent @event);
        bool Matches(IAlert alert, IEvent @event);
    }
}
