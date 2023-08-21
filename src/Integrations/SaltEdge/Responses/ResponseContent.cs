using System.Text.Json;

namespace App.Integrations.SaltEdge.Responses;

public class ResponseContent
{
    private readonly string _serializedContent;

    public T? As<T>()
    {
        return JsonSerializer.Deserialize<T>(_serializedContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public static ResponseContent From(string serializedContent)
    {
        return new ResponseContent(serializedContent);
    }

    private ResponseContent(string serializedContent)
    {
        _serializedContent = serializedContent;
    }
}
