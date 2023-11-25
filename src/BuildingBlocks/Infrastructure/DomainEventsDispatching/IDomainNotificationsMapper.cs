using Microsoft.EntityFrameworkCore;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainNotificationsMapper<TContext> where TContext : DbContext
{
    string? GetName(Type type);

    Type? GetType(string name);
}
