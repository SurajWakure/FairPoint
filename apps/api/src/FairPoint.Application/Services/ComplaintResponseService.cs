using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class ComplaintResponseService : IComplaintResponseService
{
    private readonly IComplaintResponseRepository _repository;
    private readonly IComplaintRepository _complaintRepository;
    private readonly INotificationService _notificationService;
    public ComplaintResponseService(
    IComplaintResponseRepository repository,
    INotificationService notificationService,
    IComplaintRepository complaintRepository)
    {
        _repository = repository;
        _notificationService = notificationService;
        _complaintRepository = complaintRepository;
    }

    public async Task<long> AddResponseAsync(
        long complaintId,
        long userId,
        string responseText,
        long currentUserId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(responseText))
        {
            throw new ArgumentException(
                "Response text is required.");
        }

        if (userId != currentUserId)
        {
            throw new UnauthorizedAccessException(
                "You can only respond as yourself.");
        }

        var complaint =
            await _complaintRepository.GetByIdAsync(
                complaintId,
                cancellationToken);

        if (complaint is null)
        {
            throw new KeyNotFoundException(
                "Complaint not found.");
        }

        var response = new ComplaintResponse
        {
            ComplaintId = complaintId,
            UserId = userId,
            ResponseText = responseText.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        return await _repository.AddAsync(
            response,
            cancellationToken);
    }

    public async Task<IReadOnlyList<ComplaintResponse>>
        GetResponsesAsync(
            long complaintId,
            long currentUserId,
            CancellationToken cancellationToken = default)
    {
        var complaint =
            await _complaintRepository.GetByIdAsync(
                complaintId,
                cancellationToken);

        if (complaint is null)
        {
            throw new KeyNotFoundException(
                "Complaint not found.");
        }

        return await _repository.GetByComplaintIdAsync(
            complaintId,
            cancellationToken);
    }
}