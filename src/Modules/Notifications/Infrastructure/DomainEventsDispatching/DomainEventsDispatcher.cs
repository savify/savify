using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Notifications.Infrastructure.DomainEventsDispatching;

public class DomainEventsDispatcher<TContext>(IMediator mediator, IDomainEventsAccessor<TContext> domainEventsAccessor)
    : IDomainEventsDispatcher<TContext>
    where TContext : DbContext
{
    public async Task DispatchEventsAsync()
    {
        var domainEvents = domainEventsAccessor.GetAllDomainEvents();

        domainEventsAccessor.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent)
                .ContinueWith(_ => this.DispatchEventsAsync(),
                    TaskContinuationOptions.ExecuteSynchronously);
        }

    }
}
