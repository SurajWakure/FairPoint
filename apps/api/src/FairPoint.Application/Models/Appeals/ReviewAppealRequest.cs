namespace FairPoint.Application.Models.Appeals;

public class ReviewAppealRequest
{
    public string StatusCode { get; set; } = string.Empty;

    public string DecisionReason { get; set; } = string.Empty;
}