using FairPoint.Application.Interfaces;
using FairPoint.Domain.Entities;

namespace FairPoint.Application.Services;

public class ComplaintPartyService : IComplaintPartyService
{
    private readonly IComplaintPartyRepository _complaintPartyRepository;
    private readonly IUserRepository _userRepository;

    public ComplaintPartyService(
    IComplaintPartyRepository complaintPartyRepository,
    IUserRepository userRepository)
    {
        _complaintPartyRepository = complaintPartyRepository;
        _userRepository = userRepository;
    }

    public async Task<long> AddPartyAsync(
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
    CancellationToken cancellationToken = default)
    {
        // 1. Validate UserId when a known person is supplied
        if (userId.HasValue)
        {
            var user = await _userRepository.GetByIdAsync(
                userId.Value,
                cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException(
                    "The specified user does not exist.");
            }
        }

        // 2. Identified party must have a UserId
        if (isIdentified && !userId.HasValue)
        {
            throw new InvalidOperationException(
                "An identified party must have a UserId.");
        }

        // 3. Create ComplaintParty
        var party = new ComplaintParty
        {
            ComplaintId = complaintId,
            UserId = userId,
            PartyTypeId = partyTypeId,
            IsIdentified = isIdentified,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        var complaintPartyId = await _complaintPartyRepository.AddAsync(
            party,
            cancellationToken);

        // 4. If additional identification details were supplied,
        //    save them in ComplaintPartyDetails.
        if (!string.IsNullOrWhiteSpace(vehicleNumber) ||
            !string.IsNullOrWhiteSpace(vehicleType) ||
            !string.IsNullOrWhiteSpace(vehicleColor) ||
            !string.IsNullOrWhiteSpace(description) ||
            !string.IsNullOrWhiteSpace(identificationNotes))
        {
            var details = new ComplaintPartyDetails
            {
                ComplaintPartyId = complaintPartyId,

                VehicleNumber = vehicleNumber,

                VehicleType = vehicleType,

                VehicleColor = vehicleColor,

                Description = description,

                IdentificationNotes = identificationNotes,

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = null
            };

            await _complaintPartyRepository.AddDetailsAsync(
                details,
                cancellationToken);
        }

        return complaintPartyId;
    }
}