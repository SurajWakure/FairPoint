using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IAuditLogRepository
{
    Task<long> CreateAsync(
        AuditLog auditLog,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditLog>> GetByEntityAsync(
        string entityType,
        long entityId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditLog>> GetByUserAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditLog>> GetRecentAsync(
        int take = 100,
        CancellationToken cancellationToken = default);
}