using App.Modules.Banks.Infrastructure.Configuration.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Banks.Infrastructure.Configuration.DependencyInjection;

internal static class LoggingServiceCollectionExtensions
{
    internal static IServiceCollection AddLoggingServices(this IServiceCollection services)
    {
        services.AddScoped<ILoggerProvider, LoggerProvider>();

        return services;
    }
}
