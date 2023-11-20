using App.BuildingBlocks.Infrastructure.Configuration.Inbox;
using App.BuildingBlocks.Infrastructure.Configuration.InternalCommands;
using Microsoft.Extensions.DependencyInjection;

namespace App.BuildingBlocks.Infrastructure.Configuration.Extensions;

public static class ProcessingServiceProviderExtensions
{
    public static IServiceCollection AddInternalProcessingServices(this IServiceCollection services)
    {
        services.AddScoped<InboxCommandProcessor>();
        services.AddScoped<InternalCommandProcessor>();

        return services;
    }
}
