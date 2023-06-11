using App.BuildingBlocks.Application.Events;
using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Serialization;
using MediatR;
using Newtonsoft.Json;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IMediator _mediator;

    private readonly IOutbox _outbox;

    private readonly IDomainEventsAccessor _domainEventsAccessor;

    private readonly IDomainNotificationsMapper _domainNotificationsMapper;

    public DomainEventsDispatcher(
        IMediator mediator,
        IOutbox outbox,
        IDomainEventsAccessor domainEventsAccessor,
        IDomainNotificationsMapper domainNotificationsMapper)
    {
        _mediator = mediator;
        _outbox = outbox;
        _domainEventsAccessor = domainEventsAccessor;
        _domainNotificationsMapper = domainNotificationsMapper;
    }

    public async Task DispatchEventsAsync()
    {
        var domainEvents = _domainEventsAccessor.GetAllDomainEvents();
        
        var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();
        foreach (var domainEvent in domainEvents)
        {
            Type? domainNotificationType = _domainNotificationsMapper.GetType(domainEvent.GetType().Name);
            object? domainNotification = null;
            
            if (domainNotificationType != null)
            {
                domainNotification = Activator.CreateInstance(
                    domainNotificationType,
                    domainEvent.Id,
                    domainEvent);   
            }

            if (domainNotification != null)
            {
                domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
            }
        }
        
        _domainEventsAccessor.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
        
        foreach (var domainEventNotification in domainEventNotifications)
        {
            var type = _domainNotificationsMapper.GetName(domainEventNotification.GetType());
            var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings
            {
                ContractResolver = new AllPropertiesContractResolver()
            });

            var outboxMessage = new OutboxMessage(
                domainEventNotification.Id,
                domainEventNotification.DomainEvent.OccurredOn,
                type,
                data);

            _outbox.Add(outboxMessage);
        }
    }
}
