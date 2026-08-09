using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<long> CreateAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetRolesAsync(
        long userId,
    CancellationToken cancellationToken = default);
}