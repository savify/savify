using App.Integrations.SaltEdge.Requests;
using App.Integrations.SaltEdge.Responses;
using Serilog;

namespace App.Integrations.SaltEdge.Client;

public class SaltEdgeHttpClient(HttpClient httpClient, ILogger logger) : ISaltEdgeHttpClient
{
    public async Task<Response> SendAsync(Request request)
    {
        var httpRequestMessage = new HttpRequestMessage(
            request.Method,
            request.GetFullUrl())
        {
            Content = request.Content
        };

        var requestFullUrl = httpClient.BaseAddress + request.GetFullUrl();
        logger.Information("Attempting to perform {Method}:{Url} request", request.Method, requestFullUrl);

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        var response = await Response.From(httpResponseMessage);

        if (response.IsSuccessful())
        {
            logger.Information("Request {Method}:{Url} processed successfully", request.Method, requestFullUrl);
        }
        else
        {
            logger.Warning(
                "Request {Method}:{Url} failed with {Status} HTTP code: {Message}. Error: {@Error}",
                request.Method,
                requestFullUrl,
                response.StatusCode,
                response.Error?.Message,
                response.Error);
        }

        return await Response.From(httpResponseMessage);
    }
}
