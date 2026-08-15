using FairPoint.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/moderator")]
[Authorize(Roles = "Moderator,Admin,SuperAdmin")]
public class ModeratorController : ControllerBase
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IModerationService _moderationService;
    private readonly IAppealService _appealService;

    public ModeratorController(
        IComplaintRepository complaintRepository,
        IModerationService moderationService,
        IAppealService appealService)
    {
        _complaintRepository = complaintRepository;
        _moderationService = moderationService;
        _appealService = appealService;
    }

    [HttpGet("complaints/pending")]
    public async Task<IActionResult> GetPendingComplaints(
        CancellationToken cancellationToken)
    {
        var complaints =
            await _complaintRepository
                .GetPendingForModerationAsync(
                    cancellationToken);

        return Ok(complaints);
    }

    [HttpGet("complaints/{complaintId:long}/history")]
    public async Task<IActionResult> GetModerationHistory(
        long complaintId,
        CancellationToken cancellationToken)
    {
        var history =
            await _moderationService.GetHistoryAsync(
                complaintId,
                cancellationToken);

        return Ok(history);
    }

    [HttpGet("appeals/pending")]
    public async Task<IActionResult> GetPendingAppeals(
        CancellationToken cancellationToken)
    {
        return Ok(
            await _appealService.GetPendingAsync(
                cancellationToken));
    }

    [HttpPost("appeals/{appealId:long}/review")]
    public async Task<IActionResult> ReviewAppeal(
        long appealId,
        [FromBody] ReviewAppealRequest request,
        CancellationToken cancellationToken)
    {
        var moderatorId =
            GetCurrentUserId();

        var result =
            await _appealService.ReviewAsync(
                appealId,
                moderatorId,
                request.StatusCode,
                request.DecisionReason,
                cancellationToken);

        return Ok(new
        {
            success = result
        });
    }

    private long GetCurrentUserId()
    {
        var claim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);

        if (claim == null ||
            !long.TryParse(claim.Value, out var id))
        {
            throw new UnauthorizedAccessException();
        }

        return id;
    }
}

public class ReviewAppealRequest
{
    public string StatusCode { get; set; } = string.Empty;

    public string DecisionReason { get; set; } = string.Empty;
}