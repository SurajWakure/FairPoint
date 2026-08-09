using FairPoint.Application.DTOs.Users;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<long> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default);
}