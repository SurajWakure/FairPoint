using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IModerationService
{
    Task<long> ChangeStatusAsync(
        long complaintId,
        long moderatorUserId,
        int newStatusId,
        string reason,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ModerationAction>> GetHistoryAsync(
        long complaintId,
        CancellationToken cancellationToken = default);
}