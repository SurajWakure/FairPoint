using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IModerationRepository
{
    Task<ModerationAction?> GetLastActionAsync(
        long complaintId,
        CancellationToken cancellationToken = default);

    Task<long> ChangeStatusAsync(
        long complaintId,
        long moderatorUserId,
        string actionType,
        string reason,
        int previousStatusId,
        int newStatusId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ModerationAction>> GetActionsAsync(
        long complaintId,
        CancellationToken cancellationToken = default);
}