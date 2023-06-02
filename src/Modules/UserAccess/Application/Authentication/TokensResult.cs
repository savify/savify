namespace App.Modules.UserAccess.Application.Authentication;

public class TokensResult
{
    public string AccessToken { get; set; }
    
    public string RefreshToken { get; set; }
    
    public int ExpiresIn { get; set; }

    public TokensResult(string accessToken, string refreshToken, int expiresIn)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresIn = expiresIn;
    }
}
