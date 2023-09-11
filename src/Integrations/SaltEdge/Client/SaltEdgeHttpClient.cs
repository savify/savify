using App.Integrations.SaltEdge.Requests;
using App.Integrations.SaltEdge.Responses;
using Serilog;

namespace App.Integrations.SaltEdge.Client;

public class SaltEdgeHttpClient : ISaltEdgeHttpClient
{
    private readonly HttpClient _httpClient;

    private readonly ILogger _logger;

    public SaltEdgeHttpClient(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Response> SendAsync(Request request)
    {
        var httpRequestMessage = new HttpRequestMessage(
            request.Method,
            request.GetFullUrl())
        {
            Content = request.Content
        };

        var requestFullUrl = _httpClient.BaseAddress + request.GetFullUrl();
        _logger.Information("Attempting to perform {Method}:{Url} request", request.Method, requestFullUrl);

        var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
        var response = await Response.From(httpResponseMessage);

        if (response.IsSuccessful())
        {
            _logger.Information("Request {Method}:{Url} processed successfully", request.Method, requestFullUrl);
        }
        else
        {
            _logger.Warning(
                "Request {Method}:{Url} failed with {Status} HTTP code: {Message}. Error: {@Error}",
                request.Method,
                requestFullUrl,
                response.StatusCode,
                response.Error.Message,
                response.Error);
        }

        return await Response.From(httpResponseMessage);
    }
}
