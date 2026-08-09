using FairPoint.Application.DTOs.Users;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public UserService(
        IUserRepository userRepository,
        IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<User?> GetByIdAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        if (userId <= 0)
        {
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(userId));
        }

        return await _userRepository.GetByIdAsync(
            userId,
            cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "Email is required.",
                nameof(email));
        }

        email = email.Trim().ToLowerInvariant();

        return await _userRepository.GetByEmailAsync(
            email,
            cancellationToken);
    }

    public async Task<long> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            throw new ArgumentException(
                "First name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException(
                "Email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException(
                "Password is required.");
        }

        if (request.Password.Length < 8)
        {
            throw new ArgumentException(
                "Password must contain at least 8 characters.");
        }

        var email = request.Email
            .Trim()
            .ToLowerInvariant();

        var existingUser =
            await _userRepository.GetByEmailAsync(
                email,
                cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException(
                "A user with this email already exists.");
        }

        var passwordHash =
            _passwordService.HashPassword(
                request.Password);

        var user = new User
        {
            FirstName = request.FirstName.Trim(),

            LastName = string.IsNullOrWhiteSpace(request.LastName)
                ? null
                : request.LastName.Trim(),

            Email = email,

            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber)
                ? null
                : request.PhoneNumber.Trim(),

            PasswordHash = passwordHash,

            IsActive = true,

            IsVerified = false,

            ReputationScore = 0,

            LastLoginAt = null,

            CreatedAt = DateTime.UtcNow,

            UpdatedAt = null
        };

        return await _userRepository.CreateAsync(
            user,
            cancellationToken);
    }
}