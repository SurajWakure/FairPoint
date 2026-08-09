namespace FairPoint.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(
        long userId,
        string email,
        string firstName,
        IReadOnlyList<string> roles,
        out DateTime expiresAtUtc);
}