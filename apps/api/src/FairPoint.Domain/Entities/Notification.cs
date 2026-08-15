namespace FairPoint.Domain.Entities;

public class Notification
{
    public long NotificationId { get; set; }

    public long UserId { get; set; }

    public string NotificationType { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string? RelatedEntityType { get; set; }

    public long? RelatedEntityId { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ReadAt { get; set; }
}