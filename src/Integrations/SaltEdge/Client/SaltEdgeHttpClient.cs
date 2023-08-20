using App.Integrations.SaltEdge.Configuration;
using App.Integrations.SaltEdge.Requests;
using App.Integrations.SaltEdge.Responses;

namespace App.Integrations.SaltEdge.Client;

internal class SaltEdgeHttpClient : ISaltEdgeHttpClient
{
    private readonly SaltEdgeClientConfiguration _configuration;
    
    private readonly SaltEdgeClientSecrets _secrets;
    
    private readonly HttpClient _httpClient;

    public SaltEdgeHttpClient(SaltEdgeClientConfiguration configuration, SaltEdgeClientSecrets secrets, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _secrets = secrets;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<Response> SendAsync(Request request)
    {
        var httpRequestMessage = new HttpRequestMessage(
            request.Method, 
            request.GetFullUrl(_configuration.BaseUrl))
        {
            Headers =
            {
                { HeaderName.AppId, _secrets.AppId },
                { HeaderName.Secret, _secrets.AppSecret }
            },
            Content = request.Content
        };

        var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
        
        return await Response.From(httpResponseMessage);
    }
}
