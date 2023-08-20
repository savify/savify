namespace App.Integrations.SaltEdge.Responses;

public class ResponseError
{
    public string Class { get; }

    public string Message { get; }

    public string DocumentationUrl { get; }

    public Guid RequestId { get; }

    public ResponseError(string @class, string message, string documentationUrl, Guid requestId)
    {
        Class = @class;
        Message = message;
        DocumentationUrl = documentationUrl;
        RequestId = requestId;
    }
}
