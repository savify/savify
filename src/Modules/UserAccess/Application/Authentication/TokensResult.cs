namespace App.Modules.UserAccess.Application.Authentication;

public class TokensResult
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public TokensResult(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
