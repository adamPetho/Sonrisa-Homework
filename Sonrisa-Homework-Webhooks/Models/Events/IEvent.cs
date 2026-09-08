namespace Sonrisa_Homework_Webhooks.Models.Events
{
    public interface IEvent
    {
        Guid Id { get; }
        string Type { get; }
        DateTimeOffset OccurredAt { get; }
    }
}
