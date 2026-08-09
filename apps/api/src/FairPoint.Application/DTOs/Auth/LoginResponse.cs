namespace FairPoint.Application.DTOs.Auth;

public class LoginResponse
{
    public long UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = [];

    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }
}