using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Notifications.Infrastructure.DomainEventsDispatching;

public class DomainEventsDispatcher<TContext> : IDomainEventsDispatcher<TContext> where TContext : DbContext
{
    private readonly IMediator _mediator;

    private readonly IDomainEventsAccessor<TContext> _domainEventsAccessor;

    public DomainEventsDispatcher(IMediator mediator, IDomainEventsAccessor<TContext> domainEventsAccessor)
    {
        _mediator = mediator;
        _domainEventsAccessor = domainEventsAccessor;
    }

    public async Task DispatchEventsAsync()
    {
        var domainEvents = _domainEventsAccessor.GetAllDomainEvents();

        _domainEventsAccessor.ClearAllDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }

    }
}
