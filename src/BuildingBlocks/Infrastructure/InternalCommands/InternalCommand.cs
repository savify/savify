namespace App.BuildingBlocks.Infrastructure.InternalCommands;

public class InternalCommand
{
    public Guid Id { get; set; }
    
    public Guid CausationId { get; set; }

    public string Type { get; set; }

    public string Data { get; set; }
    
    public DateTime EnqueueDate { get; set; }

    public DateTime? ProcessedDate { get; set; }
    
    public string? Error { get; set; }
}
