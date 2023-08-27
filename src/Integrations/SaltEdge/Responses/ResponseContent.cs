using System.Text.Json;
using App.Integrations.SaltEdge.Exceptions;
using App.Integrations.SaltEdge.Json;

namespace App.Integrations.SaltEdge.Responses;

public class ResponseContent
{
    private readonly string _serializedContent;

    public T? As<T>()
    {
        var responseContentData = JsonSerializer.Deserialize<ResponseContentData<T>>(_serializedContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        });

        if (responseContentData is null)
        {
            throw new InvalidResponseContentException("Response content was not properly deserialized");
        }

        return responseContentData.Data;
    }

    public static ResponseContent From(string serializedContent)
    {
        return new ResponseContent(serializedContent);
    }

    private ResponseContent(string serializedContent)
    {
        _serializedContent = serializedContent;
    }

    private class ResponseContentData<T>
    {
        public T? Data { get; set; }
    }
}
