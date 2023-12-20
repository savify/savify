namespace App.BuildingBlocks.Domain;

public abstract class DomainEventBase : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
