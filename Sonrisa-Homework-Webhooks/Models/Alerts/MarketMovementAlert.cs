namespace Sonrisa_Homework_Webhooks.Models.Alerts
{
    public sealed record MarketMovementAlert(
        Guid Id,
        string Name,
        string EventType,
        bool Enabled,
        string Symbol,
        decimal ThresholdPercentage,
        IReadOnlyCollection<string> Channels) : IAlert;
}
