using App.BuildingBlocks.Infrastructure.Authentication;
using App.Modules.UserAccess.Application.Contracts;
using IdentityServer4;
using IdentityServer4.Models;

namespace App.Modules.UserAccess.Infrastructure.IdentityServer;

public class IdentityServerConfig
{
    public static IEnumerable<ApiResource> GetApis(AuthenticationConfiguration configuration)
    {
        return new List<ApiResource>
        {
            new(configuration.ApiName, "Savify API")
        };
    }
    
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource(CustomClaimTypes.Roles, new List<string>
            {
                CustomClaimTypes.Roles
            })
        };
    }
    
    public static IEnumerable<Client> GetClients(AuthenticationConfiguration configuration)
    {
        return new List<Client>
        {
            new()
            {
                ClientId = configuration.ClientId,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Sliding,

                ClientSecrets =
                {
                    new Secret(configuration.ClientSecret.Sha256())
                },
                AllowedScopes =
                {
                    configuration.ApiName,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            }
        };
    }
}
