namespace App.Modules.UserAccess.Infrastructure.Authentication;

public class AuthenticationConfiguration
{
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string IssuerSigningKey { get; set; }

    public int AccessTokenTtl { get; set; }

    public int RefreshTokenTtl { get; set; }
}
