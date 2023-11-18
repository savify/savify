using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Integration;
using App.Modules.UserAccess.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Notifications.Infrastructure.Configuration.EventBus;

public static class EventBusInitialization
{
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }

    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
        var eventBus = CompositionRoot.BeginScope().ServiceProvider.GetRequiredService<IEventBus>();

        SubscribeToIntegrationEvent<NewUserRegisteredIntegrationEvent>(eventBus, logger);
        SubscribeToIntegrationEvent<UserRegistrationRenewedIntegrationEvent>(eventBus, logger);
        SubscribeToIntegrationEvent<UserRegistrationConfirmedIntegrationEvent>(eventBus, logger);
        SubscribeToIntegrationEvent<PasswordResetRequestedIntegrationEvent>(eventBus, logger);
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger) where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(new IntegrationEventGenericHandler<T>());
    }
}
