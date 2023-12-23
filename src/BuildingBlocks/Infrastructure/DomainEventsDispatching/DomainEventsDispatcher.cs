using App.BuildingBlocks.Application;
using App.BuildingBlocks.Application.Events;
using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Outbox;
using App.BuildingBlocks.Infrastructure.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainEventsDispatcher<TContext>(
    IMediator mediator,
    IExecutionContextAccessor executionContextAccessor,
    IOutbox<TContext> outbox,
    IDomainEventsAccessor<TContext> domainEventsAccessor,
    IDomainNotificationsMapper<TContext> domainNotificationsMapper)
    : IDomainEventsDispatcher<TContext>
    where TContext : DbContext
{
    public async Task DispatchEventsAsync()
    {
        var domainEvents = domainEventsAccessor.GetAllDomainEvents();

        var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();
        foreach (var domainEvent in domainEvents)
        {
            var domainNotificationType = domainNotificationsMapper.GetType(domainEvent.GetType().Name);
            object? domainNotification = null;

            if (domainNotificationType != null)
            {
                domainNotification = Activator.CreateInstance(
                    domainNotificationType,
                    domainEvent.Id,
                    executionContextAccessor.CorrelationId,
                    domainEvent);
            }

            if (domainNotification != null)
            {
                domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent> ?? throw new InvalidOperationException());
            }
        }

        domainEventsAccessor.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent).ContinueWith(
                _ => this.DispatchEventsAsync(),
                TaskContinuationOptions.ExecuteSynchronously);
        }

        foreach (var domainEventNotification in domainEventNotifications)
        {
            var type = domainNotificationsMapper.GetName(domainEventNotification.GetType());
            var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings
            {
                ContractResolver = new AllPropertiesContractResolver()
            });

            var outboxMessage = new OutboxMessage(
                domainEventNotification.Id,
                domainEventNotification.DomainEvent.OccurredOn,
                type!,
                data);

            outbox.Add(outboxMessage);
        }
    }
}
