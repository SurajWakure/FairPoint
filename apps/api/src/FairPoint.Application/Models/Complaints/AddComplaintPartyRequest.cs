namespace FairPoint.Application.Models.Complaints;

public class AddComplaintPartyRequest
{
    public long? UserId { get; set; }

    public int PartyTypeId { get; set; }

    public bool IsIdentified { get; set; }

    public string? VehicleNumber { get; set; }

    public string? VehicleType { get; set; }

    public string? VehicleColor { get; set; }

    public string? Description { get; set; }

    public string? IdentificationNotes { get; set; }
}