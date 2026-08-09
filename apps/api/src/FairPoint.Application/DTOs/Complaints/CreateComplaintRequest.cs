namespace FairPoint.Application.DTOs.Complaints;

public class CreateComplaintRequest
{
    public int CategoryId { get; set; }

    public int SeverityId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string VisibilityCode { get; set; } = "PUBLIC";
}