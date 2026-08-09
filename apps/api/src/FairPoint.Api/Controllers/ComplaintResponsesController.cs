using System.Security.Claims;
using FairPoint.Application.Interfaces;
using FairPoint.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/complaints/{complaintId:long}/responses")]
[Authorize]
public class ComplaintResponsesController : ControllerBase
{
    private readonly IComplaintResponseService _service;

    public ComplaintResponsesController(
        IComplaintResponseService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddResponse(
        long complaintId,
        AddComplaintResponseRequest request,
        CancellationToken cancellationToken)
    {
        var currentUserId =
            long.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);

        var responseId =
            await _service.AddResponseAsync(
                complaintId,
                currentUserId,
                request.ResponseText,
                currentUserId,
                cancellationToken);

        return Ok(new
        {
            message = "Response added successfully.",
            responseId
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetResponses(
        long complaintId,
        CancellationToken cancellationToken)
    {
        var currentUserId =
            long.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);

        var responses =
            await _service.GetResponsesAsync(
                complaintId,
                currentUserId,
                cancellationToken);

        return Ok(responses);
    }
}