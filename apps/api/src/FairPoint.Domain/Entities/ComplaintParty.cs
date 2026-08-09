namespace FairPoint.Domain.Entities;

public class ComplaintParty
{
    public long ComplaintPartyId { get; set; }

    public long ComplaintId { get; set; }

    public long? UserId { get; set; }

    public int PartyTypeId { get; set; }

    public bool IsIdentified { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}