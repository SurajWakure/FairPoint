using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IComplaintResponseRepository
{
    Task<long> AddAsync(
        ComplaintResponse response,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ComplaintResponse>> GetByComplaintIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default);

    Task<ComplaintResponse?> GetByIdAsync(
        long responseId,
        CancellationToken cancellationToken = default);

    Task<bool> SoftDeleteAsync(
        long responseId,
        long deletedByUserId,
        CancellationToken cancellationToken = default);
}