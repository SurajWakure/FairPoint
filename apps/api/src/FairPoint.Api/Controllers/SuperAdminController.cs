using FairPoint.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/superadmin")]
[Authorize(Roles = "SuperAdmin")]
public class SuperAdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public SuperAdminController(
        IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles(
        CancellationToken cancellationToken)
    {
        return Ok(
            await _adminService.GetRolesAsync(
                cancellationToken));
    }
}