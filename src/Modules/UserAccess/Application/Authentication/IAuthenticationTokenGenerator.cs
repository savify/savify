namespace App.Modules.UserAccess.Application.Authentication;

public interface IAuthenticationTokenGenerator
{
    public Token GenerateAccessToken(Guid userId);
    
    public Token GenerateRefreshToken();
    
    public Token GenerateRefreshToken(string value);
}
