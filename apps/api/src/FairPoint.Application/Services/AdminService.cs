using System.Data;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _repository;
    private readonly IAuditLogService _auditLogService;

    public AdminService(
        IAdminRepository repository,
        IAuditLogService auditLogService)
    {
        _repository = repository;
        _auditLogService = auditLogService;
    }

    public Task<IReadOnlyList<User>> GetUsersAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetUsersAsync(
            cancellationToken);
    }

    public Task<User?> GetUserAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetUserByIdAsync(
            userId,
            cancellationToken);
    }

    public async Task<bool> SetUserActiveAsync(
        long userId,
        bool isActive,
        long adminUserId,
        CancellationToken cancellationToken = default)
    {
        var success =
            await _repository.SetUserActiveAsync(
                userId,
                isActive,
                cancellationToken);

        if (!success)
            return false;

        await _auditLogService.LogAsync(
            new AuditLog
            {
                UserId = adminUserId,
                ActionType =
                    isActive
                        ? "ACTIVATE_USER"
                        : "DEACTIVATE_USER",
                EntityType = "User",
                EntityId = userId,
                NewValues = $$"""
                {
                    "isActive": {{isActive.ToString().ToLowerInvariant()}}
                }
                """
            },
            cancellationToken);

        return true;
    }

    public Task<IReadOnlyList<int>> GetUserRolesAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetUserRoleIdsAsync(
            userId,
            cancellationToken);
    }

    public async Task<bool> AssignRoleAsync(
        long userId,
        int roleId,
        long adminUserId,
        CancellationToken cancellationToken = default)
    {
        var success =
            await _repository.AssignRoleAsync(
                userId,
                roleId,
                cancellationToken);

        if (!success)
            return false;

        await _auditLogService.LogAsync(
            new AuditLog
            {
                UserId = adminUserId,
                ActionType = "ASSIGN_ROLE",
                EntityType = "User",
                EntityId = userId,
                NewValues = $$"""
                {
                    "roleId": {{roleId}}
                }
                """
            },
            cancellationToken);

        return true;
    }

    public async Task<bool> RemoveRoleAsync(
        long userId,
        int roleId,
        long adminUserId,
        CancellationToken cancellationToken = default)
    {
        var success =
            await _repository.RemoveRoleAsync(
                userId,
                roleId,
                cancellationToken);

        if (!success)
            return false;

        await _auditLogService.LogAsync(
            new AuditLog
            {
                UserId = adminUserId,
                ActionType = "REMOVE_ROLE",
                EntityType = "User",
                EntityId = userId,
                OldValues = $$"""
                {
                    "roleId": {{roleId}}
                }
                """
            },
            cancellationToken);

        return true;
    }

    public Task<IReadOnlyList<Role>> GetRolesAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetRolesAsync(
            cancellationToken);
    }
}