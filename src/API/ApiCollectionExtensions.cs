using System.Text;
using App.BuildingBlocks.Integration;
using App.Modules.UserAccess.Infrastructure.Authentication;
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

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authenticationConfiguration.Issuer,
                    ValidAudience = authenticationConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.IssuerSigningKey))
                };
            });

        services.AddSingleton<IAuthenticationConfigurationProvider>(
            _ => new AuthenticationConfigurationProvider(authenticationConfiguration));

        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, InMemoryEventBusClient>();

        return services;
    }
}
