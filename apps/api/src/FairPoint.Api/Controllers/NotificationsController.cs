using System.Security.Claims;
using FairPoint.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(
        INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotifications(
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

        var notifications =
            await _notificationService
                .GetMyNotificationsAsync(
                    userId,
                    cancellationToken);

        return Ok(notifications);
    }

    [HttpPut("{notificationId:long}/read")]
    public async Task<IActionResult> MarkAsRead(
        long notificationId,
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

        var success =
            await _notificationService.MarkAsReadAsync(
                notificationId,
                userId,
                cancellationToken);

        if (!success)
        {
            return NotFound(new
            {
                message =
                    "Notification not found or already read."
            });
        }

        return Ok(new
        {
            message = "Notification marked as read."
        });
    }
}