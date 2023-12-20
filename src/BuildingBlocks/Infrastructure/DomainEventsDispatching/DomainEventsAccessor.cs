using App.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainEventsAccessor<TContext>(TContext context) : IDomainEventsAccessor<TContext>
    where TContext : DbContext
{
    public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        return domainEntities
            .SelectMany(x => x.Entity.DomainEvents!)
            .ToList();
    }

    public void ClearAllDomainEvents()
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
    }
}
