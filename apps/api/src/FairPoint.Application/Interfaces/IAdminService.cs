using System.Data;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IAdminService
{
    Task<IReadOnlyList<User>> GetUsersAsync(
        CancellationToken cancellationToken = default);

    Task<User?> GetUserAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<bool> SetUserActiveAsync(
        long userId,
        bool isActive,
        long adminUserId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<int>> GetUserRolesAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<bool> AssignRoleAsync(
        long userId,
        int roleId,
        long adminUserId,
        CancellationToken cancellationToken = default);

    Task<bool> RemoveRoleAsync(
        long userId,
        int roleId,
        long adminUserId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Role>> GetRolesAsync(
        CancellationToken cancellationToken = default);
}