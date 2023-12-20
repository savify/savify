namespace App.BuildingBlocks.Infrastructure.Configuration.Outbox;

public class OutboxMessageDto
{
    public Guid Id { get; init; }

    public required string Type { get; init; }

    public required string Data { get; init; }
}
