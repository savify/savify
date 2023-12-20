namespace App.BuildingBlocks.Infrastructure.Configuration.Inbox;

public class InboxMessageDto
{
    public Guid Id { get; init; }

    public required string Type { get; init; }

    public required string Data { get; init; }
}
