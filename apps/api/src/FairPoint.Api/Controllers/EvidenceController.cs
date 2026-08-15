using System.Security.Claims;
using FairPoint.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/complaints/{complaintId:long}/evidence")]
[Authorize]
public class EvidenceController : ControllerBase
{
    private readonly IEvidenceService _evidenceService;

    public EvidenceController(
        IEvidenceService evidenceService)
    {
        _evidenceService = evidenceService;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(500 * 1024 * 1024)]
    public async Task<IActionResult> Upload(
        long complaintId,
        IFormFile file,
        [FromForm] string? description,
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

        if (file is null || file.Length == 0)
        {
            return BadRequest(new
            {
                message = "A file is required."
            });
        }

        try
        {
            await using var stream =
                file.OpenReadStream();

            var evidenceId =
                await _evidenceService.AddAsync(
                    complaintId,
                    currentUserId,
                    stream,
                    file.FileName,
                    file.ContentType,
                    file.Length,
                    description,
                    cancellationToken);

            return Ok(new
            {
                evidenceId,
                fileName = file.FileName,
                contentType = file.ContentType,
                fileSize = file.Length,
                message = "Evidence uploaded successfully."
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
    }

    [HttpGet]
    public async Task<IActionResult> GetByComplaintId(
    long complaintId,
    CancellationToken cancellationToken)
    {
        var evidence =
            await _evidenceService.GetByComplaintIdAsync(
                complaintId,
                cancellationToken);

        return Ok(evidence);
    }
}