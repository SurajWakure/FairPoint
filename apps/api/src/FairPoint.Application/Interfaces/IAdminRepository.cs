using System.Data;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IAdminRepository
{
    Task<IReadOnlyList<User>> GetUsersAsync(
        CancellationToken cancellationToken = default);

    Task<User?> GetUserByIdAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<bool> SetUserActiveAsync(
        long userId,
        bool isActive,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<int>> GetUserRoleIdsAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<bool> AssignRoleAsync(
        long userId,
        int roleId,
        CancellationToken cancellationToken = default);

    Task<bool> RemoveRoleAsync(
        long userId,
        int roleId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Role>> GetRolesAsync(
        CancellationToken cancellationToken = default);
}