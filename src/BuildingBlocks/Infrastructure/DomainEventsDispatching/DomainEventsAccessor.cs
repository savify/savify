using App.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainEventsAccessor : IDomainEventsAccessor
{
    private readonly DbContext _context;

    public DomainEventsAccessor(DbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
    {
        var domainEntities = _context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        return domainEntities
            .SelectMany(x => x.Entity.DomainEvents!)
            .ToList();
    }

    public void ClearAllDomainEvents()
    {
        var domainEntities = _context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
    }
}
