namespace App.Integrations.SaltEdge.Configuration;

public class SaltEdgeClientConfiguration
{
    public required string BaseUrl { get; set; }

    public required string AppId { get; set; }

    public required string AppSecret { get; set; }
}
