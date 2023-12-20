namespace App.Integrations.SaltEdge.Responses;

public class ResponseError(string @class, string message, string documentationUrl, Guid requestId)
{
    public string Class { get; } = @class;

    public string Message { get; } = message;

    public string DocumentationUrl { get; } = documentationUrl;

    public Guid RequestId { get; } = requestId;
}
