using App.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainEventsAccessor<TContext> where TContext : DbContext
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

    void ClearAllDomainEvents();
}
