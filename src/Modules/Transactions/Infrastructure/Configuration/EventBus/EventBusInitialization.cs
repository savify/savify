using App.BuildingBlocks.Integration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Transactions.Infrastructure.Configuration.EventBus;

public static class EventBusInitialization
{
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }

    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
        var eventBus = TransactionsCompositionRoot.BeginScope().ServiceProvider.GetRequiredService<IEventBus>();

        // SubscribeToIntegrationEvent<SomeIntegrationEvent>(eventBus, logger);
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger) where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(new IntegrationEventGenericHandler<T>());
    }
}
