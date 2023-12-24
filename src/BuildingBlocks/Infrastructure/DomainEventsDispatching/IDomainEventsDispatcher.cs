using Microsoft.EntityFrameworkCore;

namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainEventsDispatcher<TContext> where TContext : DbContext
{
    Task DispatchEventsAsync();
}
