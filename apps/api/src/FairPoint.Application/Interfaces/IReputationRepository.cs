using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IReputationRepository
{
    Task<long> AddHistoryAsync(
        ReputationHistory history,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReputationHistory>> GetHistoryAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<int> GetCurrentScoreAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task UpdateUserScoreAsync(
        long userId,
        int pointsChange,
        CancellationToken cancellationToken = default);
}