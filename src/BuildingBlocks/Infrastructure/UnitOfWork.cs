using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Microsoft.EntityFrameworkCore;

namespace App.BuildingBlocks.Infrastructure;

public class UnitOfWork<TContext>(TContext context, IDomainEventsDispatcher<TContext> domainEventsDispatcher)
    : IUnitOfWork<TContext>
    where TContext : DbContext
{
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default, Guid? internalCommandId = null)
    {
        domainEventsDispatcher.DispatchEventsAsync();

        try
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
