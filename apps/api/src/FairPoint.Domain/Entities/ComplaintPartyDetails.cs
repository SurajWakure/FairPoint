namespace FairPoint.Domain.Entities;

public class ComplaintPartyDetails
{
    public long ComplaintPartyDetailId { get; set; }

    public long ComplaintPartyId { get; set; }

    public string? VehicleNumber { get; set; }

    public string? VehicleType { get; set; }

    public string? VehicleColor { get; set; }

    public string? Description { get; set; }

    public string? IdentificationNotes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}