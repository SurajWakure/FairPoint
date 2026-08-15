using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class ReputationService : IReputationService
{
    private readonly IReputationRepository _repository;

    public ReputationService(
        IReputationRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddPointsAsync(
        long userId,
        int points,
        string reasonCode,
        string? description,
        long? complaintId,
        long? createdByUserId,
        CancellationToken cancellationToken = default)
    {
        if (userId <= 0)
        {
            throw new ArgumentException(
                "Invalid user.");
        }

        if (string.IsNullOrWhiteSpace(reasonCode))
        {
            throw new ArgumentException(
                "Reason code is required.");
        }

        await _repository.UpdateUserScoreAsync(
            userId,
            points,
            cancellationToken);

        await _repository.AddHistoryAsync(
            new ReputationHistory
            {
                UserId = userId,
                ComplaintId = complaintId,
                PointsChange = points,
                ReasonCode = reasonCode,
                Description = description,
                CreatedByUserId = createdByUserId
            },
            cancellationToken);

        return await _repository.GetCurrentScoreAsync(
            userId,
            cancellationToken);
    }

    public Task<int> GetCurrentScoreAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetCurrentScoreAsync(
            userId,
            cancellationToken);
    }

    public Task<IReadOnlyList<ReputationHistory>>
        GetHistoryAsync(
            long userId,
            CancellationToken cancellationToken = default)
    {
        return _repository.GetHistoryAsync(
            userId,
            cancellationToken);
    }
}