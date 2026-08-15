using System.Security.Claims;
using FairPoint.Application.Interfaces;
using FairPoint.Application.Models.Moderation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/complaints/{complaintId:long}/moderation")]
[Authorize(Roles = "User,Moderator,Admin,SuperAdmin")]
[Authorize]
public class ModerationController : ControllerBase
{
    private readonly IModerationService _moderationService;

    public ModerationController(
        IModerationService moderationService)
    {
        _moderationService = moderationService;
    }

    [HttpPost("status")]
    public async Task<IActionResult> ChangeStatus(
        long complaintId,
        ChangeComplaintStatusRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (!long.TryParse(
                userIdClaim,
                out var currentUserId))
        {
            return Unauthorized();
        }

        try
        {
            var actionId =
                await _moderationService.ChangeStatusAsync(
                    complaintId,
                    currentUserId,
                    request.NewStatusId,
                    request.Reason,
                    cancellationToken);

            return Ok(new
            {
                moderationActionId = actionId,
                message = "Complaint status updated successfully."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(
        long complaintId,
        CancellationToken cancellationToken)
    {
        var history =
            await _moderationService.GetHistoryAsync(
                complaintId,
                cancellationToken);

        return Ok(history);
    }
}