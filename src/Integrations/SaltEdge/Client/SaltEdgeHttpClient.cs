using App.Integrations.SaltEdge.Configuration;
using App.Integrations.SaltEdge.Requests;
using App.Integrations.SaltEdge.Responses;
using Serilog;

namespace App.Integrations.SaltEdge.Client;

public class SaltEdgeHttpClient : ISaltEdgeHttpClient
{
    private readonly SaltEdgeClientConfiguration _configuration;

    private readonly HttpClient _httpClient;

    private readonly ILogger _logger;

    public SaltEdgeHttpClient(SaltEdgeClientConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger logger)
    {
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }

    public async Task<Response> SendAsync(Request request)
    {
        var requestFullUrl = request.GetFullUrl(_configuration.BaseUrl);
        var httpRequestMessage = new HttpRequestMessage(
            request.Method,
            requestFullUrl)
        {
            Headers =
            {
                { HeaderName.AppId, _configuration.AppId },
                { HeaderName.Secret, _configuration.AppSecret }
            },
            Content = request.Content
        };

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
