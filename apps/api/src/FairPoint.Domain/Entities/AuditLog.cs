namespace FairPoint.Domain.Entities;

public class AuditLog
{
    public long AuditLogId { get; set; }

    public long? UserId { get; set; }

    public string ActionType { get; set; } = string.Empty;

    public string? EntityType { get; set; }

    public long? EntityId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; }
}