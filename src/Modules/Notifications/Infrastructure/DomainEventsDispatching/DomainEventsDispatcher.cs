using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using MediatR;

namespace App.Modules.Notifications.Infrastructure.DomainEventsDispatching;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IMediator _mediator;

    private readonly IDomainEventsAccessor _domainEventsAccessor;

    public DomainEventsDispatcher(IMediator mediator, IDomainEventsAccessor domainEventsAccessor)
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
