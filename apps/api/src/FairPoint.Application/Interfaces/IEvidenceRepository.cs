using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IEvidenceRepository
{
    Task<long> AddAsync(
        Evidence evidence,
        CancellationToken cancellationToken = default);

    Task<Evidence?> GetByIdAsync(
        long evidenceId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Evidence>> GetByComplaintIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default);
}