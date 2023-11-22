using App.BuildingBlocks.Application.Events;
using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.BuildingBlocks.Infrastructure.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainEventsDispatcher<TContext> : IDomainEventsDispatcher<TContext> where TContext : DbContext
{
    private readonly IMediator _mediator;

    private readonly IOutbox<TContext> _outbox;

    private readonly IDomainEventsAccessor<TContext> _domainEventsAccessor;

    private readonly IDomainNotificationsMapper<TContext> _domainNotificationsMapper;

    public DomainEventsDispatcher(
        IMediator mediator,
        IOutbox<TContext> outbox,
        IDomainEventsAccessor<TContext> domainEventsAccessor,
        IDomainNotificationsMapper<TContext> domainNotificationsMapper)
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
                domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent> ?? throw new InvalidOperationException());
            }
        }

        _domainEventsAccessor.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent).ContinueWith(async _ =>
            {
                await this.DispatchEventsAsync();
            });
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
                type!,
                data);

            _outbox.Add(outboxMessage);
        }
    }
}
