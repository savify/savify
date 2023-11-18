using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.Logging;

internal static class LoggingModule
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<ICategoriesLoggerProvider, CategoriesLoggerProvider>();
    }
}
