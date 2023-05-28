using App.API.Modules.UserAccess.Authentication;
using App.BuildingBlocks.Infrastructure.Authentication;
using App.Modules.UserAccess.Infrastructure.IdentityServer;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace App.API;

public static class ApiCollectionExtensions
{
    public static IServiceCollection AddUserAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authenticationConfiguration = configuration.GetSection("Authentication").Get<AuthenticationConfiguration>();
        
        services.AddIdentityServer()
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfig.GetApis(authenticationConfiguration))
            .AddInMemoryClients(IdentityServerConfig.GetClients(authenticationConfiguration))
            .AddInMemoryPersistedGrants()
            .AddProfileService<ProfileService>()
            .AddDeveloperSigningCredential();

        services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authenticationConfiguration.Authority;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = authenticationConfiguration.Authority,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });

        services.AddSingleton<IAuthenticationConfigurationProvider>(
            _ => new AuthenticationConfigurationProvider(authenticationConfiguration));

        return services;
    }
}
