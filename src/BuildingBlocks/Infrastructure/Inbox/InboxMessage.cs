namespace App.BuildingBlocks.Infrastructure.Inbox;

public class InboxMessage(Guid id, DateTime occurredOn, string type, string data)
{
    public Guid Id { get; init; } = id;

    public DateTime OccurredOn { get; init; } = occurredOn;

    public string Type { get; init; } = type;

    public string Data { get; init; } = data;

    public DateTime? ProcessedDate { get; init; }
}
