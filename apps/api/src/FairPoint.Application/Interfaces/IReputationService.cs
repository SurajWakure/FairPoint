using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IReputationService
{
    Task<int> AddPointsAsync(
        long userId,
        int points,
        string reasonCode,
        string? description,
        long? complaintId,
        long? createdByUserId,
        CancellationToken cancellationToken = default);

    Task<int> GetCurrentScoreAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReputationHistory>> GetHistoryAsync(
        long userId,
        CancellationToken cancellationToken = default);
}