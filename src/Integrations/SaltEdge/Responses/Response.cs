using System.Net;
using System.Text.Json;
using App.Integrations.SaltEdge.Exceptions;

namespace App.Integrations.SaltEdge.Responses;

public abstract class Response
{
    public HttpStatusCode StatusCode { get; }

    protected Response(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public static async Task<Response> From(HttpResponseMessage responseMessage)
    {
        if (responseMessage.IsSuccessStatusCode)
        {
            return await SuccessResponse.CreateFrom(responseMessage);
        }

        return await ErrorResponse.CreateFrom(responseMessage);
    }

    public bool IsSuccessful() => (int)StatusCode >= 200 && (int)StatusCode <= 299;
}

public class SuccessResponse : Response
{
    public ResponseContent Content { get; private set; }

    public static async Task<SuccessResponse> CreateFrom(HttpResponseMessage responseMessage)
    {
        var serializedResponseContent = await responseMessage.Content.ReadAsStringAsync();
        var content = ResponseContent.From(serializedResponseContent);

        return new SuccessResponse(responseMessage.StatusCode, content);
    }

    private SuccessResponse(HttpStatusCode statusCode, ResponseContent content) : base(statusCode)
    {
        Content = content;
    }
}

public class ErrorResponse : Response
{
    public ResponseError Error { get; private set; }

    public static async Task<ErrorResponse> CreateFrom(HttpResponseMessage responseMessage)
    {
        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ResponseError>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return new ErrorResponse(responseMessage.StatusCode, error ?? throw new InvalidResponseContentException("Response content was not properly deserialized"));
    }

    private ErrorResponse(HttpStatusCode statusCode, ResponseError error) : base(statusCode)
    {
        Error = error;
    }
}
