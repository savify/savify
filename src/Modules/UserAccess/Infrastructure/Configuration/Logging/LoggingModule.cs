using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Logging;

internal static class LoggingModule
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<ILoggerProvider, LoggerProvider>();
    }
}
