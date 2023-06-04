using App.BuildingBlocks.Infrastructure.Authentication;
using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Authentication;

internal static class AuthenticationModule
{
    internal static void Configure(IServiceCollection services, AuthenticationConfiguration configuration)
    {
        services.AddSingleton<IAuthenticationConfigurationProvider>(_ => new AuthenticationConfigurationProvider(configuration));
        
        services.AddScoped<IAuthenticationTokenGenerator, AuthenticationTokenGenerator>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
}
