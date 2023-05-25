namespace App.BuildingBlocks.Infrastructure.DomainEventsDispatching;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}
