namespace App.Modules.UserAccess.Infrastructure.Authentication;

public class AuthenticationConfiguration
{
    public required string Issuer { get; set; }

    public required string Audience { get; set; }

    public required string IssuerSigningKey { get; set; }

    public int AccessTokenTtl { get; set; }

    public int RefreshTokenTtl { get; set; }
}
