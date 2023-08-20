using System.Net;
using System.Text.Json;

namespace App.Integrations.SaltEdge.Responses;

public abstract class Response
{
    public HttpStatusCode StatusCode { get; protected set; }

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

    public bool IsSuccessful() => this is SuccessResponse;
}

public class SuccessResponse : Response
{
    public object Data { get; private set; }

    public static async Task<SuccessResponse> CreateFrom(HttpResponseMessage responseMessage)
    {
        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<object>(responseContent);

        return new SuccessResponse(responseMessage.StatusCode, data ?? throw new InvalidOperationException());
    }

    private SuccessResponse(HttpStatusCode statusCode, object data) : base(statusCode)
    {
        Data = data;
    }
}

public class ErrorResponse : Response
{
    public ResponseError Error { get; private set; }

    public static async Task<ErrorResponse> CreateFrom(HttpResponseMessage responseMessage)
    {
        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ResponseError>(responseContent);

        return new ErrorResponse(responseMessage.StatusCode, error ?? throw new InvalidOperationException());
    }

    private ErrorResponse(HttpStatusCode statusCode, ResponseError error) : base(statusCode)
    {
        Error = error;
    }
}
