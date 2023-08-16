namespace App.BuildingBlocks.Infrastructure.InternalCommands;

public class InternalCommand
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!;

    public string Data { get; set; } = null!;

    public DateTime EnqueueDate { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public string? Error { get; set; }
}
