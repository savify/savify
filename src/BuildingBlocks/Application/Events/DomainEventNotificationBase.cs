using App.BuildingBlocks.Domain;

namespace App.BuildingBlocks.Application.Events;

public class DomainEventNotificationBase<T> : IDomainEventNotification<T> where T : IDomainEvent
{
    public Guid Id { get; }

    public Guid CorrelationId { get; }

    public T DomainEvent { get; }

    public DomainEventNotificationBase(Guid id, Guid correlationId, T domainEvent)
    {
        Id = id;
        CorrelationId = correlationId;
        DomainEvent = domainEvent;
    }
}
