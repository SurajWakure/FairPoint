namespace FairPoint.Domain.Entities;

public class User
{
	public long UserId { get; set; }

	public string FirstName { get; set; } = string.Empty;

	public string? LastName { get; set; }

	public string Email { get; set; } = string.Empty;

	public string? PhoneNumber { get; set; }

	public string PasswordHash { get; set; } = string.Empty;

	public bool IsActive { get; set; }

	public bool IsVerified { get; set; }

	public int ReputationScore { get; set; }

	public DateTime? LastLoginAt { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime? UpdatedAt { get; set; }
}