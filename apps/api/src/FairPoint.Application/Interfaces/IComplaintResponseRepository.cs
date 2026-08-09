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
}