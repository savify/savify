using Microsoft.EntityFrameworkCore;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainNotificationsMapper<TContext>(BiDictionary<string, Type> domainNotificationsMap)
    : IDomainNotificationsMapper<TContext>
    where TContext : DbContext
{
    public string? GetName(Type type)
    {
        return domainNotificationsMap.TryGetBySecond(type, out var name) ? name : null;
    }

    public Type? GetType(string name)
    {
        return domainNotificationsMap.TryGetByFirst(name, out var type) ? type : null;
    }
}
