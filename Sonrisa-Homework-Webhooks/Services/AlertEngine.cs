using Sonrisa_Homework_Webhooks.Models;
using Sonrisa_Homework_Webhooks.Models.Alerts;
using Sonrisa_Homework_Webhooks.Models.Events;
using Sonrisa_Homework_Webhooks.Repositories;
using Sonrisa_Homework_Webhooks.Services.AlertEvaluators;

namespace Sonrisa_Homework_Webhooks.Services
{
    public sealed class AlertEngine : IAlertEngine
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IAlertEvaluator _alertEvaluator;
        private readonly IMessageQueue _messageQueue;

        public AlertEngine(
            IAlertRepository alertRepository,
            IAlertEvaluator alertEvaluator,
            IMessageQueue messageQueue)
        {
            _alertRepository = alertRepository;
            _alertEvaluator = alertEvaluator;
            _messageQueue = messageQueue;
        }

        public async Task ProcessEventAsync(
            IEvent @event,
            CancellationToken cancellationToken)
        {
            var alerts = _alertRepository
                .GetAll()
                .Where(x => x.Enabled)
                .Where(x => x.EventType == @event.Type);

            foreach (var alert in alerts)
            {
                if (!_alertEvaluator.Matches(alert, @event))
                    continue;

                foreach (var channel in GetChannels(alert))
                {
                    var notification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        AlertId = alert.Id,
                        EventId = @event.Id,
                        Channel = channel,
                        Subject = alert.Name,
                        Message = BuildMessage(@event),
                        CreatedAt = DateTimeOffset.UtcNow,
                        Status = NotificationStatus.Pending
                    };

                    await _messageQueue.EnqueueAsync(notification, cancellationToken);
                }
            }
        }

        private static string BuildMessage(IEvent @event)
        {
            return @event switch
            {
                MarketMovementEvent market =>
                    $"{market.Symbol} moved {market.ChangePercentage:+0.##;-0.##;0}%.",
    
                _ => $"Event occurred: {@event.Type}"
            };
        }

        private static IEnumerable<string> GetChannels(IAlert alert)
        {
            return alert switch
            {
                MarketMovementAlert market => market.Channels,
                _ => []
            };
        }
    }
}
