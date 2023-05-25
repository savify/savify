using App.BuildingBlocks.Domain;

namespace App.BuildingBlocks.Application.Events;

public class DomainEventNotificationBase<T> : IDomainEventNotification<T> where T : IDomainEvent
{
    public Guid Id { get; }
    
    public T DomainEvent { get; }

    public DomainEventNotificationBase(Guid id, T domainEvent)
    {
        Id = id;
        DomainEvent = domainEvent;
    }
}
