namespace FairPoint.Domain.Entities;

public class Evidence
{
    public long EvidenceId { get; set; }

    public long ComplaintId { get; set; }

    public long UploadedByUserId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string StorageKey { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string? FileHash { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? DeletedByUserId { get; set; }
}