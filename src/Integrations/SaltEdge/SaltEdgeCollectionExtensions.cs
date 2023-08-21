using App.Integrations.SaltEdge.Client;
using App.Integrations.SaltEdge.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Integrations.SaltEdge;

public static class SaltEdgeCollectionExtensions
{
    public static IServiceCollection AddSaltEdgeIntegration(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        var integrationLogger = logger.ForContext("Integration", "SaltEdge");

        services.AddScoped<ISaltEdgeHttpClient>(provider => new SaltEdgeHttpClient(
            configuration.GetSection("SaltEdge").Get<SaltEdgeClientConfiguration>(),
            provider.GetService<IHttpClientFactory>(),
            integrationLogger
            ));

        return services;
    }
}
