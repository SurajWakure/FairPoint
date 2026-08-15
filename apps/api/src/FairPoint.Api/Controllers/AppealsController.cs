using System.Security.Claims;
using FairPoint.Application.Interfaces;
using FairPoint.Application.Models.Appeals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/appeals")]
[Authorize]
public class AppealsController : ControllerBase
{
    private readonly IAppealService _appealService;

    public AppealsController(
        IAppealService appealService)
    {
        _appealService = appealService;
    }

    [HttpPost("complaints/{complaintId:long}")]
    public async Task<IActionResult> Submit(
        long complaintId,
        SubmitAppealRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (!long.TryParse(
                userIdClaim,
                out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var appealId =
                await _appealService.SubmitAsync(
                    complaintId,
                    userId,
                    request.Reason,
                    cancellationToken);

            return Ok(new
            {
                appealId,
                message = "Appeal submitted successfully."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("{appealId:long}")]
    public async Task<IActionResult> GetById(
        long appealId,
        CancellationToken cancellationToken)
    {
        var appeal =
            await _appealService.GetByIdAsync(
                appealId,
                cancellationToken);

        if (appeal == null)
        {
            return NotFound();
        }

        return Ok(appeal);
    }

    [HttpGet("complaints/{complaintId:long}")]
    public async Task<IActionResult> GetByComplaint(
        long complaintId,
        CancellationToken cancellationToken)
    {
        var appeals =
            await _appealService.GetByComplaintIdAsync(
                complaintId,
                cancellationToken);

        return Ok(appeals);
    }

    [HttpGet("pending")]
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public async Task<IActionResult> GetPending(
        CancellationToken cancellationToken)
    {
        var appeals =
            await _appealService.GetPendingAsync(
                cancellationToken);

        return Ok(appeals);
    }

    [HttpPut("{appealId:long}/review")]
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public async Task<IActionResult> Review(
        long appealId,
        ReviewAppealRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (!long.TryParse(
                userIdClaim,
                out var reviewerId))
        {
            return Unauthorized();
        }

        try
        {
            await _appealService.ReviewAsync(
                appealId,
                reviewerId,
                request.StatusCode,
                request.DecisionReason,
                cancellationToken);

            return Ok(new
            {
                message = "Appeal reviewed successfully."
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
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }
}