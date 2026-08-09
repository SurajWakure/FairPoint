using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IComplaintRepository
{
    Task<long> CreateAsync(
        Complaint complaint,
        CancellationToken cancellationToken = default);

    Task<Complaint?> GetByIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Complaint>> GetByUserIdAsync(
        long userId,
        CancellationToken cancellationToken = default);
}