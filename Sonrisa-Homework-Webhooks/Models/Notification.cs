namespace Sonrisa_Homework_Webhooks.Models
{
    public sealed class Notification
    {
        public Guid Id { get; init; }

        public Guid AlertId { get; init; }

        public Guid EventId { get; init; }

        public string Channel { get; init; } = null!;

        public string Subject { get; init; } = null!;

        public string Message { get; init; } = null!;

        public DateTimeOffset CreatedAt { get; init; }

        public NotificationStatus Status { get; set; }
    }
    public enum NotificationStatus
    {
        Pending,
        Sent,
        Failed
    }
}
