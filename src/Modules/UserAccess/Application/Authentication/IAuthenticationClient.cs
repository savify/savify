using IdentityModel.Client;

namespace App.Modules.UserAccess.Application.Authentication;

public interface IAuthenticationClient
{
    public Task<TokenResponse> RequestTokens(string email, string password);
    
    public Task<TokenResponse> RefreshTokens(string refreshToken);
}
