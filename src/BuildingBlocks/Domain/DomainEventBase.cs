namespace App.BuildingBlocks.Domain;

public abstract class DomainEventBase : IDomainEvent
{
    public Guid Id { get; }

    public DateTime OccurredOn { get; }

    protected DomainEventBase()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}
