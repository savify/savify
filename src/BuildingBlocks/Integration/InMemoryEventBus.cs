namespace App.BuildingBlocks.Integration;

public sealed class InMemoryEventBus
{
    public static InMemoryEventBus Instance { get; } = new InMemoryEventBus();

    private readonly IDictionary<string, List<IIntegrationEventHandler>> _handlersDictionary;

    static InMemoryEventBus()
    {
    }

    private InMemoryEventBus()
    {
        _handlersDictionary = new Dictionary<string, List<IIntegrationEventHandler>>();
    }

    public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
    {
        var eventType = typeof(T).FullName;
        if (eventType != null)
        {
            if (_handlersDictionary.TryGetValue(eventType, out var handlers))
            {
                handlers.Add(handler);
            }
            else
            {
                _handlersDictionary.Add(eventType, [handler]);
            }
        }
    }

    public async Task Publish<T>(T @event) where T : IntegrationEvent
    {
        var eventType = @event.GetType().FullName;

        if (eventType == null)
        {
            return;
        }

        var integrationEventHandlers = _handlersDictionary[eventType];

        foreach (var integrationEventHandler in integrationEventHandlers)
        {
            if (integrationEventHandler is IIntegrationEventHandler<T> handler)
            {
                await handler.Handle(@event);
            }
        }
    }
}
