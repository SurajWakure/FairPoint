namespace FairPoint.Domain.Entities;

public class ReputationHistory
{
    public long ReputationHistoryId { get; set; }

    public long UserId { get; set; }

    public long? ComplaintId { get; set; }

    public int PointsChange { get; set; }

    public string ReasonCode { get; set; } = string.Empty;

    public string? Description { get; set; }

    public long? CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }
}