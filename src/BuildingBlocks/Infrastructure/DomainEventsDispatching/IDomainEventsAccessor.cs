using App.BuildingBlocks.Domain;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainEventsAccessor
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

    void ClearAllDomainEvents();
}
