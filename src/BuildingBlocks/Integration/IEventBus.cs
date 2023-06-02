namespace App.BuildingBlocks.Integration;

public interface IEventBus : IDisposable
{
    Task Publish<T>(T @event) where T : IntegrationEvent;
    
    void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent;
}
