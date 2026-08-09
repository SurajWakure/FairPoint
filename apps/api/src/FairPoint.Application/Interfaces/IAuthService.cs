using FairPoint.Application.DTOs.Auth;

namespace FairPoint.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default);
}