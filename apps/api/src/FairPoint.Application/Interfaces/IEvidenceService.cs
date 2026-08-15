using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IEvidenceService
{
    Task<long> AddAsync(
        long complaintId,
        long uploadedByUserId,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize,
        string? description,
        CancellationToken cancellationToken = default);

    Task<Evidence?> GetByIdAsync(
        long evidenceId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Evidence>> GetByComplaintIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default);
}