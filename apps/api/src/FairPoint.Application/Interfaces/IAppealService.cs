using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IAppealService
{
    Task<long> SubmitAsync(
        long complaintId,
        long submittedByUserId,
        string reason,
        CancellationToken cancellationToken = default);

    Task<Appeal?> GetByIdAsync(
        long appealId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Appeal>> GetByComplaintIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Appeal>> GetPendingAsync(
        CancellationToken cancellationToken = default);

    Task<bool> ReviewAsync(
        long appealId,
        long reviewedByUserId,
        string statusCode,
        string decisionReason,
        CancellationToken cancellationToken = default);
}