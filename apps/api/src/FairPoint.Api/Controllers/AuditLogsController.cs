using FairPoint.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/audit-logs")]
[Authorize(
    Roles = "Moderator,Admin,SuperAdmin")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogsController(
        IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    // GET: api/audit-logs/recent
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecent(
        [FromQuery] int take = 100,
        CancellationToken cancellationToken = default)
    {
        var logs =
            await _auditLogService.GetRecentAsync(
                take,
                cancellationToken);

        return Ok(logs);
    }

    // GET: api/audit-logs/entity/Complaint/1
    [HttpGet("entity/{entityType}/{entityId:long}")]
    public async Task<IActionResult> GetByEntity(
        string entityType,
        long entityId,
        CancellationToken cancellationToken)
    {
        var logs =
            await _auditLogService.GetByEntityAsync(
                entityType,
                entityId,
                cancellationToken);

        return Ok(logs);
    }

    // GET: api/audit-logs/user/1
    [HttpGet("user/{userId:long}")]
    public async Task<IActionResult> GetByUser(
        long userId,
        CancellationToken cancellationToken)
    {
        var logs =
            await _auditLogService.GetByUserAsync(
                userId,
                cancellationToken);

        return Ok(logs);
    }
}