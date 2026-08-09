using FairPoint.Domain.Entities;

namespace FairPoint.Application.Interfaces;

public interface IComplaintPartyRepository
{
    Task<long> AddAsync(
        ComplaintParty party,
        CancellationToken cancellationToken = default);

    Task AddDetailsAsync(
        ComplaintPartyDetails details,
        CancellationToken cancellationToken = default);
}