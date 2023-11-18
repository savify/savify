using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Transactions.Infrastructure.Configuration.Logging;

internal static class LoggingModule
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<ILoggerProvider, LoggerProvider>();
    }
}
