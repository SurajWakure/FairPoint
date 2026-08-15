namespace FairPoint.Domain.Entities;

public class ModerationAction
{
    public long ModerationActionId { get; set; }

    public long ComplaintId { get; set; }

    public long ModeratorUserId { get; set; }

    public string ActionType { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    public int? PreviousStatusId { get; set; }

    public int? NewStatusId { get; set; }

    public DateTime CreatedAt { get; set; }
}