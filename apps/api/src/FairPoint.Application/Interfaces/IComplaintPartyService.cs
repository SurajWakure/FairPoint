namespace FairPoint.Application.Interfaces;

public interface IComplaintPartyService
{
    Task<long> AddPartyAsync(
    long complaintId,
    long? userId,
    int partyTypeId,
    bool isIdentified,
    string? vehicleNumber,
    string? vehicleType,
    string? vehicleColor,
    string? description,
    string? identificationNotes,
    long currentUserId,
    CancellationToken cancellationToken = default);
    
}