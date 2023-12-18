using Serilog;

namespace App.BuildingBlocks.Integration;

public class InMemoryEventBusClient(ILogger logger) : IEventBus
{
    public void Dispose() { }

    public Task Publish<T>(T @event) where T : IntegrationEvent
    {
        logger.Information("Publishing {Event}", @event.GetType().FullName);
        return InMemoryEventBus.Instance.Publish(@event);
    }

    public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
    {
        InMemoryEventBus.Instance.Subscribe(handler);
    }
}
