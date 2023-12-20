using App.Integrations.SaltEdge.Client;
using App.Integrations.SaltEdge.Configuration;
using App.Integrations.SaltEdge.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace App.Integrations.SaltEdge;

public static class SaltEdgeCollectionExtensions
{
    public static IServiceCollection AddSaltEdgeIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        var saltEdgeClientConfiguration = configuration.GetSection("SaltEdge").Get<SaltEdgeClientConfiguration>()!;

        services.AddHttpClient<ISaltEdgeHttpClient, SaltEdgeHttpClient>(client =>
            {
                client.BaseAddress = new Uri(saltEdgeClientConfiguration.BaseUrl);
                client.DefaultRequestHeaders.Add(HeaderName.AppId, saltEdgeClientConfiguration.AppId);
                client.DefaultRequestHeaders.Add(HeaderName.Secret, saltEdgeClientConfiguration.AppSecret);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }
}
