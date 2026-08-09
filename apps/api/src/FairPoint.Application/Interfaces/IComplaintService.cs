using FairPoint.Application.DTOs.Complaints;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IComplaintService
{
    Task<Complaint> CreateAsync(
        long userId,
        CreateComplaintRequest request,
        CancellationToken cancellationToken = default);

    Task<Complaint?> GetByIdAsync(
        long complaintId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Complaint>> GetMyComplaintsAsync(
        long userId,
        CancellationToken cancellationToken = default);
}