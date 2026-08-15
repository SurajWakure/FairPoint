using System.Security.Claims;
using FairPoint.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _service;

    public AdminController(
        IAdminService service)
    {
        _service = service;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        CancellationToken cancellationToken)
    {
        return Ok(
            await _service.GetUsersAsync(
                cancellationToken));
    }

    [HttpGet("users/{userId:long}")]
    public async Task<IActionResult> GetUser(
        long userId,
        CancellationToken cancellationToken)
    {
        var user =
            await _service.GetUserAsync(
                userId,
                cancellationToken);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut("users/{userId:long}/active")]
    public async Task<IActionResult> SetActive(
        long userId,
        [FromQuery] bool isActive,
        CancellationToken cancellationToken)
    {
        var adminId = GetCurrentUserId();

        var result =
            await _service.SetUserActiveAsync(
                userId,
                isActive,
                adminId,
                cancellationToken);

        if (!result)
            return NotFound();

        return Ok(new
        {
            message = isActive
                ? "User activated."
                : "User deactivated."
        });
    }

    [HttpGet("users/{userId:long}/roles")]
    public async Task<IActionResult> GetUserRoles(
        long userId,
        CancellationToken cancellationToken)
    {
        return Ok(
            await _service.GetUserRolesAsync(
                userId,
                cancellationToken));
    }

    [HttpPost("users/{userId:long}/roles/{roleId:int}")]
    public async Task<IActionResult> AssignRole(
        long userId,
        int roleId,
        CancellationToken cancellationToken)
    {
        var adminId = GetCurrentUserId();

        var result =
            await _service.AssignRoleAsync(
                userId,
                roleId,
                adminId,
                cancellationToken);

        return result
            ? Ok(new { message = "Role assigned." })
            : Conflict(new { message = "Role already assigned." });
    }

    [HttpDelete("users/{userId:long}/roles/{roleId:int}")]
    public async Task<IActionResult> RemoveRole(
        long userId,
        int roleId,
        CancellationToken cancellationToken)
    {
        var adminId = GetCurrentUserId();

        var result =
            await _service.RemoveRoleAsync(
                userId,
                roleId,
                adminId,
                cancellationToken);

        return result
            ? Ok(new { message = "Role removed." })
            : NotFound();
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles(
        CancellationToken cancellationToken)
    {
        return Ok(
            await _service.GetRolesAsync(
                cancellationToken));
    }

    private long GetCurrentUserId()
    {
        var claim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (!long.TryParse(claim, out var userId))
        {
            throw new UnauthorizedAccessException();
        }

        return userId;
    }
}