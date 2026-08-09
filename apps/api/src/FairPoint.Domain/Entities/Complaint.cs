namespace FairPoint.Domain.Entities;

public class Complaint
{
    public long ComplaintId { get; set; }

    public string ComplaintNumber { get; set; } = string.Empty;

    public long CreatedByUserId { get; set; }

    public int CategoryId { get; set; }

    public int StatusId { get; set; }

    public int SeverityId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string VisibilityCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public DateTime? ClosedAt { get; set; }
}