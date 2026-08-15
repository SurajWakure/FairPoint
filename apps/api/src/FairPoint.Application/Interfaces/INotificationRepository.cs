using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface INotificationRepository
{
    Task<long> CreateAsync(
        Notification notification,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Notification>> GetByUserIdAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<bool> MarkAsReadAsync(
        long notificationId,
        long userId,
        CancellationToken cancellationToken = default);
}