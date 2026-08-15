using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _repository;

    public AuditLogService(
        IAuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> LogAsync(
        AuditLog auditLog,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(
                auditLog.ActionType))
        {
            throw new ArgumentException(
                "Audit action type is required.");
        }

        if (auditLog.ActionType.Length > 100)
        {
            throw new ArgumentException(
                "Audit action type cannot exceed 100 characters.");
        }

        if (auditLog.EntityType?.Length > 100)
        {
            throw new ArgumentException(
                "Entity type cannot exceed 100 characters.");
        }

        return await _repository.CreateAsync(
            auditLog,
            cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLog>>
        GetByEntityAsync(
            string entityType,
            long entityId,
            CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(entityType))
        {
            throw new ArgumentException(
                "Entity type is required.");
        }

        if (entityId <= 0)
        {
            throw new ArgumentException(
                "Invalid entity ID.");
        }

        return await _repository.GetByEntityAsync(
            entityType,
            entityId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLog>>
        GetByUserAsync(
            long userId,
            CancellationToken cancellationToken = default)
    {
        if (userId <= 0)
        {
            throw new ArgumentException(
                "Invalid user ID.");
        }

        return await _repository.GetByUserAsync(
            userId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLog>>
        GetRecentAsync(
            int take = 100,
            CancellationToken cancellationToken = default)
    {
        return await _repository.GetRecentAsync(
            take,
            cancellationToken);
    }
}