using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class ModerationService : IModerationService
{
    private readonly IModerationRepository _repository;
    private readonly IAuditLogService _auditLogService;
    private readonly INotificationService _notificationService;
    private readonly IComplaintRepository _complaintRepository;
    private readonly IReputationService _reputationService;

    public ModerationService(
        IModerationRepository repository,
        IAuditLogService auditLogService,
        INotificationService notificationService,
        IComplaintRepository complaintRepository,
        IReputationService reputationService)
    {
        _repository = repository;
        _auditLogService = auditLogService;
        _notificationService = notificationService;
        _complaintRepository = complaintRepository;
        _reputationService = reputationService;
    }


    public async Task<long> ChangeStatusAsync(
     long complaintId,
     long moderatorUserId,
     int newStatusId,
     string reason,
     CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException(
                "Reason is required.");
        }

        if (newStatusId < 1 || newStatusId > 10)
        {
            throw new ArgumentException(
                "Invalid complaint status.");
        }

        var complaint =
            await _complaintRepository.GetByIdAsync(
                complaintId,
                cancellationToken);
        if (complaint != null &&
    complaint.CreatedByUserId != moderatorUserId)
        {
            await _notificationService.CreateAsync(
                new Notification
                {
                    UserId = complaint.CreatedByUserId,
                    NotificationType = "COMPLAINT_STATUS",
                    Title = "Complaint status updated",
                    Message =
                        $"Your complaint status has changed to status {newStatusId}.",
                    RelatedEntityType = "Complaint",
                    RelatedEntityId = complaintId
                },
                cancellationToken);
        }

        if (complaint is null)
        {
            throw new KeyNotFoundException(
                "Complaint not found.");
        }

        var lastAction =
            await _repository.GetLastActionAsync(
                complaintId,
                cancellationToken);

        var previousStatusId =
            lastAction?.NewStatusId ?? 2;

        if (previousStatusId == newStatusId)
        {
            throw new InvalidOperationException(
                "Complaint is already in the requested status.");
        }

        var actionType =
            GetActionType(newStatusId);

        var moderationActionId =
            await _repository.ChangeStatusAsync(
                complaintId,
                moderatorUserId,
                actionType,
                reason,
                previousStatusId,
                newStatusId,
                cancellationToken);

        var complaint1 =
    await _complaintRepository.GetByIdAsync(
        complaintId,
        cancellationToken);

        if (complaint1 == null)
        {
            throw new KeyNotFoundException(
                "Complaint not found.");
        }

        if (complaint1.CreatedByUserId != moderatorUserId)
        {
            var statusName = newStatusId switch
            {
                1 => "Draft",
                2 => "Submitted",
                3 => "Under Review",
                4 => "Needs Information",
                5 => "Responded",
                6 => "Verified",
                7 => "Rejected",
                8 => "Resolved",
                9 => "Closed",
                10 => "Appealed",
                _ => "Updated"
            };

            await _notificationService.CreateAsync(
                new Notification
                {
                    UserId = complaint.CreatedByUserId,
                    NotificationType = "COMPLAINT_STATUS",
                    Title = "Complaint status updated",
                    Message =
                        $"Your complaint status has changed to {statusName}.",
                    RelatedEntityType = "Complaint",
                    RelatedEntityId = complaintId
                },
                cancellationToken);
        }

        await _auditLogService.LogAsync(
            new AuditLog
            {
                UserId = moderatorUserId,

                ActionType = "CHANGE_STATUS",

                EntityType = "Complaint",

                EntityId = complaintId,

                OldValues = $$"""
            {
                "statusId": {{previousStatusId}}
            }
            """,

                NewValues = $$"""
            {
                "statusId": {{newStatusId}}
            }
            """,

                IpAddress = null,

                UserAgent = null
            },
            cancellationToken);

        // VERIFIED = +10 reputation points
        if (newStatusId == 6)
        {
            await _reputationService.AddPointsAsync(
                complaint.CreatedByUserId,
                10,
                "COMPLAINT_VERIFIED",
                "Complaint was verified by moderation.",
                complaintId,
                moderatorUserId,
                cancellationToken);
        }

        return moderationActionId;
    }

    private static string GetStatusName(
    int statusId)
    {
        return statusId switch
        {
            1 => "Draft",
            2 => "Submitted",
            3 => "Under Review",
            4 => "Needs Information",
            5 => "Responded",
            6 => "Verified",
            7 => "Rejected",
            8 => "Resolved",
            9 => "Closed",
            10 => "Appealed",

            _ => "Unknown"
        };
    }

    public async Task<IReadOnlyList<ModerationAction>>
        GetHistoryAsync(
            long complaintId,
            CancellationToken cancellationToken = default)
    {
        return await _repository.GetActionsAsync(
            complaintId,
            cancellationToken);
    }

    private static string GetActionType(
        int statusId)
    {
        return statusId switch
        {
            1 => "DRAFT",
            2 => "SUBMIT",
            3 => "START_REVIEW",
            4 => "REQUEST_INFORMATION",
            5 => "RESPONSE_RECEIVED",
            6 => "VERIFY",
            7 => "REJECT",
            8 => "RESOLVE",
            9 => "CLOSE",
            10 => "APPEAL",

            _ => "STATUS_CHANGE"
        };
    }
}