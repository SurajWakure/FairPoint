using System.Security.Claims;
using FairPoint.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/reputation")]
[Authorize]
public class ReputationController : ControllerBase
{
    private readonly IReputationService _service;

    public ReputationController(
        IReputationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyReputation(
        CancellationToken cancellationToken)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var score =
            await _service.GetCurrentScoreAsync(
                userId,
                cancellationToken);

        var history =
            await _service.GetHistoryAsync(
                userId,
                cancellationToken);

        return Ok(new
        {
            score,
            history
        });
    }
}