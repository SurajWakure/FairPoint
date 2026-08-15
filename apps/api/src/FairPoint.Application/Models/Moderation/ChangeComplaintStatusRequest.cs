namespace FairPoint.Application.Models.Moderation;

public class ChangeComplaintStatusRequest
{
    public int NewStatusId { get; set; }

    public string Reason { get; set; } = string.Empty;
}