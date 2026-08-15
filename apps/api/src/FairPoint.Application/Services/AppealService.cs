using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class AppealService : IAppealService
{
    private readonly IAppealRepository _repository;
    private readonly IAuditLogService _auditLogService;
    private readonly IComplaintRepository _complaintRepository;
    private readonly INotificationService _notificationService;

    public AppealService(
    IAppealRepository repository,
    IAuditLogService auditLogService,
    IComplaintRepository complaintRepository,
    INotificationService notificationService)
    {
        _repository = repository;
        _auditLogService = auditLogService;
        _complaintRepository = complaintRepository;
        _notificationService = notificationService;
    }

    public async Task<long> SubmitAsync(
        long complaintId,
        long submittedByUserId,
        string reason,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException(
                "Appeal reason is required.");
        }

        if (reason.Length > 3000)
        {
            throw new ArgumentException(
                "Appeal reason cannot exceed 3000 characters.");
        }

        var appeal = new Appeal
        {
            ComplaintId = complaintId,
            SubmittedByUserId = submittedByUserId,
            Reason = reason,
            StatusCode = "SUBMITTED"
        };

        var appealId =
            await _repository.CreateAsync(
                appeal,
                cancellationToken);
        await _notificationService.CreateAsync(
            new Notification
            {
                UserId = submittedByUserId,
                NotificationType = "APPEAL_SUBMITTED",
                Title = "Appeal Submitted",
                Message = "Your appeal has been submitted successfully.",
                RelatedEntityType = "Appeal",
                RelatedEntityId = appealId,
                IsRead = false
            },
            cancellationToken);

        await _auditLogService.LogAsync(
            new AuditLog
            {
                UserId = submittedByUserId,
                ActionType = "SUBMIT_APPEAL",
                EntityType = "Appeal",
                EntityId = appealId,
                NewValues = $$"""
                {
                    "complaintId": {{complaintId}},
                    "statusCode": "SUBMITTED"
                }
                """
            },
            cancellationToken);

        return appealId;
    }

    public async Task<Appeal?> GetByIdAsync(
        long appealId,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(
            appealId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Appeal>>
        GetByComplaintIdAsync(
            long complaintId,
            CancellationToken cancellationToken = default)
    {
        return await _repository.GetByComplaintIdAsync(
            complaintId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Appeal>>
        GetPendingAsync(
            CancellationToken cancellationToken = default)
    {
        return await _repository.GetPendingAsync(
            cancellationToken);
    }

    public async Task<bool> ReviewAsync(
        long appealId,
        long reviewedByUserId,
        string statusCode,
        string decisionReason,
        CancellationToken cancellationToken = default)
    {
        if (statusCode != "APPROVED" &&
            statusCode != "REJECTED")
        {
            throw new ArgumentException(
                "Appeal status must be APPROVED or REJECTED.");
        }

        if (string.IsNullOrWhiteSpace(decisionReason))
        {
            throw new ArgumentException(
                "Decision reason is required.");
        }

        var appeal =
            await _repository.GetByIdAsync(
                appealId,
                cancellationToken);


        if (appeal == null)
        {
            throw new KeyNotFoundException(
                "Appeal not found.");
        }

        if (appeal.StatusCode != "SUBMITTED")
        {
            throw new InvalidOperationException(
                "This appeal has already been reviewed.");
        }

        var success =
            await _repository.ReviewAsync(
                appealId,
                reviewedByUserId,
                statusCode,
                decisionReason,
                cancellationToken);
        if (statusCode == "APPROVED")
        {
            var complaintUpdated =
                await _complaintRepository.UpdateStatusAsync(
                    appeal.ComplaintId,
                    3,
                    cancellationToken);

            if (!complaintUpdated)
            {
                throw new InvalidOperationException(
                    "Unable to move complaint back into review.");
            }
        }
        if (!success)
        {
            throw new InvalidOperationException(
                "Unable to review appeal.");
        }
        var notificationMessage =
    statusCode == "APPROVED"
        ? "Your appeal has been approved."
        : "Your appeal has been rejected.";

        await _notificationService.CreateAsync(
            new Notification
            {
                UserId = appeal.SubmittedByUserId,
                NotificationType = "APPEAL_REVIEW",
                Title = "Appeal reviewed",
                Message = notificationMessage,
                RelatedEntityType = "Appeal",
                RelatedEntityId = appealId
            },
            cancellationToken);
        await _notificationService.CreateAsync(
    new Notification
    {
        UserId = appeal.SubmittedByUserId,
        NotificationType = "APPEAL_REVIEWED",
        Title = $"Appeal {statusCode}",
        Message = $"Your appeal has been {statusCode.ToLowerInvariant()}.",
        RelatedEntityType = "Appeal",
        RelatedEntityId = appealId,
        IsRead = false
    },
    cancellationToken);

        await _auditLogService.LogAsync(
            new AuditLog
            {
                UserId = reviewedByUserId,
                ActionType = "REVIEW_APPEAL",
                EntityType = "Appeal",
                EntityId = appealId,

                OldValues = """
                {
                    "statusCode": "SUBMITTED"
                }
                """,

                NewValues = $$"""
                {
                    "statusCode": "{{statusCode}}",
                    "decisionReason": "{{decisionReason}}"
                }
                """
            },
            cancellationToken);

        return true;
    }
}