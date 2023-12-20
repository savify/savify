namespace App.Modules.UserAccess.Application.Authentication.RefreshTokens;

public class RefreshTokenDto
{
    public Guid UserId { get; set; }

    public required string Value { get; set; }

    public DateTime ExpiresAt { get; set; }
}
