using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IComplaintResponseService
{
    Task<long> AddResponseAsync(
        long complaintId,
        long userId,
        string responseText,
        long currentUserId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ComplaintResponse>> GetResponsesAsync(
        long complaintId,
        long currentUserId,
        CancellationToken cancellationToken = default);
}