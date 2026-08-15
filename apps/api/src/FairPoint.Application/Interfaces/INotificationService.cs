using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface INotificationService
{
    Task<long> CreateAsync(
        Notification notification,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Notification>> GetMyNotificationsAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<bool> MarkAsReadAsync(
        long notificationId,
        long userId,
        CancellationToken cancellationToken = default);
}