using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(
        INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> CreateAsync(
        Notification notification,
        CancellationToken cancellationToken = default)
    {
        if (notification.UserId <= 0)
        {
            throw new ArgumentException(
                "Invalid user.");
        }

        if (string.IsNullOrWhiteSpace(notification.Title))
        {
            throw new ArgumentException(
                "Notification title is required.");
        }

        if (string.IsNullOrWhiteSpace(notification.Message))
        {
            throw new ArgumentException(
                "Notification message is required.");
        }

        return await _repository.CreateAsync(
            notification,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Notification>>
        GetMyNotificationsAsync(
            long userId,
            CancellationToken cancellationToken = default)
    {
        return await _repository.GetByUserIdAsync(
            userId,
            cancellationToken);
    }

    public async Task<bool> MarkAsReadAsync(
        long notificationId,
        long userId,
        CancellationToken cancellationToken = default)
    {
        return await _repository.MarkAsReadAsync(
            notificationId,
            userId,
            cancellationToken);
    }
}