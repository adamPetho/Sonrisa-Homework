using Sonrisa_Homework_Webhooks.Models.Alerts;
using Sonrisa_Homework_Webhooks.Models.Events;

namespace Sonrisa_Homework_Webhooks.Services.AlertEvaluators
{
    public sealed class MarketMovementAlertEvaluator : IAlertEvaluator
    {
        public bool CanEvaluate(IEvent @event)
            => @event is MarketMovementEvent;

        public bool Matches(IAlert alert, IEvent @event)
        {
            if (@event is not MarketMovementEvent marketEvent)
                return false;

            if (alert is not MarketMovementAlert marketAlert)
                return false;

            if (marketAlert.Symbol != marketEvent.Symbol)
                return false;

            if (Math.Abs(marketEvent.ChangePercentage) >= marketAlert.ThresholdPercentage)
                return true;

            return false;
        }
    }
}
