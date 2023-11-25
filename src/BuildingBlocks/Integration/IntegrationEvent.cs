using MediatR;

namespace App.BuildingBlocks.Integration;

public class IntegrationEvent : INotification
{
    public Guid Id { get; }

    public Guid CorrelationId { get; }

    public DateTime OccurredOn { get; }

    protected IntegrationEvent(Guid id, Guid correlationId, DateTime occurredOn)
    {
        Id = id;
        CorrelationId = correlationId;
        OccurredOn = occurredOn;
    }
}
