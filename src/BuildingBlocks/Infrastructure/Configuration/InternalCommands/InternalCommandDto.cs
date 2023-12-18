namespace App.BuildingBlocks.Infrastructure.Configuration.InternalCommands;

public class InternalCommandDto
{
    public Guid Id { get; init; }

    public required string Type { get; init; }

    public required string Data { get; init; }
}
