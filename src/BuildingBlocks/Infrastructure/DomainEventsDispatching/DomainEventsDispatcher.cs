using App.BuildingBlocks.Application.Events;
using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IMediator _mediator;

    private readonly IServiceScope _scope;

    private readonly IOutbox _outbox;

    private readonly IDomainEventsAccessor _domainEventsAccessor;

    private readonly IDomainNotificationsMapper _domainNotificationsMapper;

    public DomainEventsDispatcher(
        IMediator mediator,
        IServiceScope scope,
        IOutbox outbox,
        IDomainEventsAccessor domainEventsAccessor,
        IDomainNotificationsMapper domainNotificationsMapper)
    {
        _mediator = mediator;
        _scope = scope;
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
            Type domainEvenNotificationType = typeof(IDomainEventNotification<>);
            var domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());

            // TODO: changed from Autofac - could not work!!!
            var domainNotification = ActivatorUtilities.CreateInstance(
                _scope.ServiceProvider,
                domainNotificationWithGenericType,
                domainEvent.Id,
                domainEvent
            );

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
            // TODO: changed from Newtonsoft.Json - could not work!!!
            var data = JsonSerializer.Serialize(domainEventNotification);

            var outboxMessage = new OutboxMessage(
                domainEventNotification.Id,
                domainEventNotification.DomainEvent.OccurredOn,
                type,
                data);

            _outbox.Add(outboxMessage);
        }
    }
}