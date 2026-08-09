using FairPoint.Application.Services;
using FairPoint.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using FairPoint.Application.DTOs.Users;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        IUserService userService,
        ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("{userId:long}")]
    public async Task<IActionResult> GetById(
        long userId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting user with ID {UserId}",
            userId);

        var user = await _userService.GetByIdAsync(
            userId,
            cancellationToken);

        if (user is null)
        {
            _logger.LogInformation(
                "User with ID {UserId} was not found",
                userId);

            return NotFound(new
            {
                message = "User not found."
            });
        }

        return Ok(ToResponse(user));
    }

    [HttpGet("by-email")]
    public async Task<IActionResult> GetByEmail(
        [FromQuery] string email,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting user by email.");

        var user = await _userService.GetByEmailAsync(
            email,
            cancellationToken);

        if (user is null)
        {
            return NotFound(new
            {
                message = "User not found."
            });
        }

        return Ok(ToResponse(user));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
    [FromBody] RegisterUserRequest request,
    CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "New user registration attempt for {Email}",
            request.Email);

        try
        {
            var userId = await _userService.RegisterAsync(
                request,
                cancellationToken);

            _logger.LogInformation(
                "User registered successfully with ID {UserId}",
                userId);

            return CreatedAtAction(
                nameof(GetById),
                new { userId },
                new
                {
                    userId,
                    message = "User registered successfully."
                });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid user registration request.");

            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "User registration failed because the email already exists.");

            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    private static object ToResponse(User user)
    {
        return new
        {
            user.UserId,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.IsActive,
            user.IsVerified,
            user.ReputationScore,
            user.LastLoginAt,
            user.CreatedAt,
            user.UpdatedAt
        };
    }
}