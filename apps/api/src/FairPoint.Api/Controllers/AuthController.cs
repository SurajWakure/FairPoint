using FairPoint.Application.DTOs.Auth;
using FairPoint.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FairPoint.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Login attempt for {Email}",
            request.Email);

        var result = await _authService.LoginAsync(
            request,
            cancellationToken);

        if (result is null)
        {
            _logger.LogWarning(
                "Login failed for {Email}",
                request.Email);

            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        _logger.LogInformation(
            "User {UserId} logged in successfully.",
            result.UserId);

        return Ok(result);
    }
}