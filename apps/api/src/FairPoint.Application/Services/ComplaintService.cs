using FairPoint.Application.DTOs.Complaints;
using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class ComplaintService : IComplaintService
{
    private readonly IComplaintRepository _complaintRepository;

    public ComplaintService(
        IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<Complaint> CreateAsync(
        long userId,
        CreateComplaintRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException(
                "Complaint title is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            throw new ArgumentException(
                "Complaint description is required.");
        }

        var complaint = new Complaint
        {
            ComplaintNumber = GenerateComplaintNumber(),

            CreatedByUserId = userId,

            CategoryId = request.CategoryId,

            StatusId = 2, // SUBMITTED

            SeverityId = request.SeverityId,

            Title = request.Title.Trim(),

            Description = request.Description.Trim(),

            VisibilityCode = request.VisibilityCode,

            CreatedAt = DateTime.UtcNow
        };

        var complaintId =
            await _complaintRepository.CreateAsync(
                complaint,
                cancellationToken);

        complaint.ComplaintId = complaintId;

        return complaint;
    }

    public async Task<Complaint?> GetByIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default)
    {
        return await _complaintRepository.GetByIdAsync(
            complaintId,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Complaint>> GetMyComplaintsAsync(
        long userId,
        CancellationToken cancellationToken = default)
    {
        return await _complaintRepository.GetByUserIdAsync(
            userId,
            cancellationToken);
    }

    private static string GenerateComplaintNumber()
    {
        return $"FP-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
    }
}