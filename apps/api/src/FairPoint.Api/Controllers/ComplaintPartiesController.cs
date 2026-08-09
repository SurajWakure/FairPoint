using System.Security.Claims;
using FairPoint.Application.Interfaces;
using FairPoint.Application.Models.Complaints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/complaints/{complaintId:long}/parties")]
[Authorize]
public class ComplaintPartiesController : ControllerBase
{
    private readonly IComplaintPartyService _partyService;

    public ComplaintPartiesController(
        IComplaintPartyService partyService)
    {
        _partyService = partyService;
    }

    [HttpPost]
    public async Task<IActionResult> AddParty(
    long complaintId,
    AddComplaintPartyRequest request,
    CancellationToken cancellationToken)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!long.TryParse(userIdClaim, out var currentUserId))
        {
            return Unauthorized();
        }

        try
        {
            var partyId =
                await _partyService.AddPartyAsync(
                    complaintId,
                    request.UserId,
                    request.PartyTypeId,
                    request.IsIdentified,
                    request.VehicleNumber,
                    request.VehicleType,
                    request.VehicleColor,
                    request.Description,
                    request.IdentificationNotes,
                    currentUserId,
                    cancellationToken);

            return Ok(new
            {
                complaintPartyId = partyId,
                message = "Complaint party added successfully."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}