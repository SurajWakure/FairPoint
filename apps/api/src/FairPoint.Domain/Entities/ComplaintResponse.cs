namespace FairPoint.Domain.Entities;

public class ComplaintResponse
{
    public long ResponseId { get; set; }

    public long ComplaintId { get; set; }

    public long UserId { get; set; }

    public string ResponseText { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? DeletedByUserId { get; set; }
}