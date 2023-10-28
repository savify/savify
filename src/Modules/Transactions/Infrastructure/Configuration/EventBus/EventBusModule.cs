using App.BuildingBlocks.Integration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Transactions.Infrastructure.Configuration.EventBus;

internal static class EventBusModule
{
    internal static void Configure(IServiceCollection services, IEventBus? eventBus = null)
    {
        if (eventBus != null)
        {
            services.AddSingleton(eventBus);
        }
        else
        {
            services.AddSingleton<IEventBus, InMemoryEventBusClient>();
        }
    }
}
