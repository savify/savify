namespace App.Modules.UserAccess.Application.Authentication;

public interface IAuthenticationTokenGenerator
{
    public Token GenerateAccessToken(Guid userId);
    
    public Token GenerateAccessToken(Guid userId, DateTime expires);
    
    public Token GenerateRefreshToken();
    
    public Token GenerateRefreshToken(string value);
}
