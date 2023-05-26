using App.BuildingBlocks.Infrastructure.Authentication;
using App.Modules.UserAccess.Application.Authentication;
using IdentityModel.Client;

namespace App.Modules.UserAccess.Infrastructure.IdentityServer;

public class AuthenticationClient : IAuthenticationClient
{
    private readonly HttpClient _client;
    private readonly AuthenticationConfiguration _configuration;

    public AuthenticationClient(IAuthenticationConfigurationProvider authenticationConfigurationProvider)
    {
        _client = new HttpClient();
        _configuration = authenticationConfigurationProvider.GetConfiguration();
    }

    public async Task<TokenResponse> RequestTokens(string email, string password)
    {
        return await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = _configuration.Address,

            ClientId = _configuration.ClientId,
            ClientSecret = _configuration.ClientSecret,

            UserName = email,
            Password = password
        });
    }

    public async Task<TokenResponse> RefreshTokens(string refreshToken)
    {
        return await _client.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = _configuration.Address,

            ClientId = _configuration.ClientId,
            ClientSecret = _configuration.ClientSecret,

            RefreshToken = refreshToken
        });
    }
}
