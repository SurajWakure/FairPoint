namespace FairPoint.Domain.Entities;

public class Appeal
{
    public long AppealId { get; set; }

    public long ComplaintId { get; set; }

    public long SubmittedByUserId { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string StatusCode { get; set; } = "SUBMITTED";

    public long? ReviewedByUserId { get; set; }

    public string? DecisionReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }
}