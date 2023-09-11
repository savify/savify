using System.Net;
using System.Text.Json;
using App.Integrations.SaltEdge.Exceptions;
using App.Integrations.SaltEdge.Json;

namespace App.Integrations.SaltEdge.Responses;

public class Response
{
    public HttpStatusCode StatusCode { get; }

    public ResponseContent? Content { get; private set; }

    public ResponseError? Error { get; private set; }

    private Response(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    private Response(HttpStatusCode statusCode, ResponseContent? content)
    {
        StatusCode = statusCode;
        Content = content;
    }

    private Response(HttpStatusCode statusCode, ResponseError? error)
    {
        StatusCode = statusCode;
        Error = error;
    }

    public static async Task<Response> From(HttpResponseMessage responseMessage)
    {
        if (!responseMessage.IsSuccessStatusCode)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var errorResponseContent = JsonSerializer.Deserialize<ErrorResponseContent>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            });

            if (errorResponseContent?.Error is null)
            {
                throw new InvalidResponseContentException("Response content was not properly deserialized");
            }

            return new Response(responseMessage.StatusCode, errorResponseContent.Error);
        }

        var serializedResponseContent = await responseMessage.Content.ReadAsStringAsync();
        var content = ResponseContent.From(serializedResponseContent);

        return new Response(responseMessage.StatusCode, content);
    }

    public bool IsSuccessful() => (int)StatusCode >= 200 && (int)StatusCode <= 299;

    private class ErrorResponseContent
    {
        public ResponseError Error { get; set; }
    }
}
