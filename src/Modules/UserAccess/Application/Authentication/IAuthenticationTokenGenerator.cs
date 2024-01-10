namespace App.Modules.UserAccess.Application.Authentication;

public interface IAuthenticationTokenGenerator
{
    public Token GenerateAccessToken(Guid userId, AccessTokenType type = AccessTokenType.Authentication);

    public Token GenerateRefreshToken();

    public Token GenerateRefreshToken(string value);
}
