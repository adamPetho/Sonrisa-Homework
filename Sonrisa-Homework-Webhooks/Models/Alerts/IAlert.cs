namespace Sonrisa_Homework_Webhooks.Models.Alerts
{
    public interface IAlert
    {
        Guid Id { get; }
        string Name { get; }
        string EventType { get; }
        bool Enabled { get; }
        IReadOnlyCollection<string> Channels { get; }
    }
}
