using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Notifications.Infrastructure.Configuration.Logging;

internal static class LoggingModule
{
    internal static void Configure(IServiceCollection services, ILogger logger)
    {
        services.AddSingleton(logger);
    }
}
