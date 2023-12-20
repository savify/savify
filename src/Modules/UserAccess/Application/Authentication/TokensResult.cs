namespace App.Modules.UserAccess.Application.Authentication;

public class TokensResult(string accessToken, string refreshToken)
{
    public string AccessToken { get; set; } = accessToken;

    public string RefreshToken { get; set; } = refreshToken;
}
