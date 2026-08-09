using System.Security.Claims;
using FairPoint.Application.DTOs.Complaints;
using FairPoint.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/complaints")]
[Authorize]
public class ComplaintController : ControllerBase
{
    private readonly IComplaintService _complaintService;

    public ComplaintController(
        IComplaintService complaintService)
    {
        _complaintService = complaintService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateComplaintRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var complaint =
            await _complaintService.CreateAsync(
                userId,
                request,
                cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { complaintId = complaint.ComplaintId },
            complaint);
    }

    [HttpGet("{complaintId:long}")]
    public async Task<IActionResult> GetById(
        long complaintId,
        CancellationToken cancellationToken)
    {
        var complaint =
            await _complaintService.GetByIdAsync(
                complaintId,
                cancellationToken);

        if (complaint == null)
        {
            return NotFound(new
            {
                message = "Complaint not found."
            });
        }

        return Ok(complaint);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyComplaints(
        CancellationToken cancellationToken)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var complaints =
            await _complaintService.GetMyComplaintsAsync(
                userId,
                cancellationToken);

        return Ok(complaints);
    }
}