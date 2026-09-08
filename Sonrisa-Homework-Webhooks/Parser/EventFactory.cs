using Sonrisa_Homework_Webhooks.Models.Events;
using System.Text.Json;

namespace Sonrisa_Homework_Webhooks.Parser
{
    public sealed class EventFactory : IEventFactory
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public IEvent Create(EventRequest request)
        {
            return request.Type.ToLowerInvariant() switch
            {
                "market.movement" => CreateMarketMovement(request.Data),

                _ => throw new NotSupportedException(
                    $"Event type '{request.Type}' is not supported.")
            };
        }

        private static MarketMovementEvent CreateMarketMovement(JsonElement data)
        {
            var marketEvent = data.Deserialize<MarketMovementData>(JsonOptions) ?? throw new JsonException("Invalid market movement data.");

            return new MarketMovementEvent(
                Guid.NewGuid(),
                marketEvent.Symbol,
                marketEvent.ChangePercentage,
                marketEvent.OccurredAt);
        }
    }

    public sealed record MarketMovementData(string Symbol, decimal ChangePercentage, DateTimeOffset OccurredAt);
}
