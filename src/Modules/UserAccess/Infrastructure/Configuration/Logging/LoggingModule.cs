using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Logging;

internal static class LoggingModule
{
    internal static void Configure(IServiceCollection services, ILogger logger)
    {
        services.AddSingleton(logger);
    }
}
