namespace Sonrisa_Homework_Webhooks.Models.Events
{
    public sealed record MarketMovementEvent(
        Guid Id,
        string Symbol,
        decimal ChangePercentage,
        DateTimeOffset OccurredAt) : IEvent
    {
        public string Type => "market.movement";
    }
}