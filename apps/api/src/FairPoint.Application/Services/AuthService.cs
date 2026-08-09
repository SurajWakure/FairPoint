using FairPoint.Application.DTOs.Auth;
using FairPoint.Application.Interfaces;

namespace FairPoint.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        var email = request.Email
            .Trim()
            .ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (user is null)
        {
            return null;
        }

        if (!user.IsActive)
        {
            return null;
        }

        var passwordValid =
            _passwordService.VerifyPassword(
                request.Password,
                user.PasswordHash);

        if (!passwordValid)
        {
            return null;
        }

        var roles = await _userRepository.GetRolesAsync(
            user.UserId,
            cancellationToken);

        var token = _jwtService.GenerateToken(
            user.UserId,
            user.Email,
            user.FirstName,
            roles,
            out var expiresAtUtc);

        return new LoginResponse
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            Email = user.Email,
            Roles = roles.ToList(),
            AccessToken = token,
            ExpiresAtUtc = expiresAtUtc
        };
    }
}