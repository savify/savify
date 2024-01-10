using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Authentication.RefreshTokens;
using App.Modules.UserAccess.Application.Authentication.TokenInvalidation;
using App.Modules.UserAccess.Infrastructure.Authentication;
using App.Modules.UserAccess.Infrastructure.Authentication.TokenInvalidation;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.Infrastructure.Configuration.DependencyInjection;

internal static class AuthenticationServiceCollectionExtensions
{
    internal static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        AuthenticationConfiguration configuration)
    {
        services.AddSingleton<IAuthenticationConfigurationProvider>(_ => new AuthenticationConfigurationProvider(configuration));

        services.AddScoped<IAuthenticationTokenGenerator, AuthenticationTokenGenerator>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IInvalidatedAccessTokenRepository, InvalidatedAccessTokenRepository>();
        services.AddScoped<IAccessTokenInvalidator, AccessTokenInvalidator>();

        return services;
    }
}
