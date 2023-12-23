using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.EventBus;
using App.BuildingBlocks.Integration;
using App.Modules.Categories.IntegrationEvents;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.EventBus;

public static class EventBusInitialization
{
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }

    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
        var eventBus = CompositionRoot.BeginScope().ServiceProvider.GetRequiredService<IEventBus>();

        SubscribeToIntegrationEvent<NewCategoryCreatedIntegrationEvent>(eventBus, logger);
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger) where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(new IntegrationEventGenericHandler<T>(DatabaseConfiguration.Schema));
    }
}
