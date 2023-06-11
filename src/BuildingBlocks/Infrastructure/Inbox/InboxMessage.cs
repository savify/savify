namespace App.BuildingBlocks.Infrastructure.Inbox;

public class InboxMessage
{
    public Guid Id { get; set; }

    public DateTime OccurredOn { get; set; }

    public string Type { get; set; } = null!;

    public string Data { get; set; } = null!;

    public DateTime? ProcessedDate { get; set; }

    public InboxMessage(DateTime occurredOn, string type, string data)
    {
        Id = Guid.NewGuid();
        OccurredOn = occurredOn;
        Type = type;
        Data = data;
    }

    private InboxMessage()
    {
    }
}
