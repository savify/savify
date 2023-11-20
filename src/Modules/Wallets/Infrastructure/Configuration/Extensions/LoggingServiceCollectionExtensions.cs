using App.Modules.Wallets.Infrastructure.Configuration.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Wallets.Infrastructure.Configuration.Extensions;

internal static class LoggingServiceCollectionExtensions
{
    internal static IServiceCollection AddLoggingServices(this IServiceCollection services)
    {
        services.AddScoped<ILoggerProvider, LoggerProvider>();

        return services;
    }
}
