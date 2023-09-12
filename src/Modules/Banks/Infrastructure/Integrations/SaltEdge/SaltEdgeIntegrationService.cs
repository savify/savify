using App.Integrations.SaltEdge.Client;
using App.Integrations.SaltEdge.Requests;
using App.Modules.Banks.Infrastructure.Integrations.SaltEdge.Providers;

namespace App.Modules.Banks.Infrastructure.Integrations.SaltEdge;

public class SaltEdgeIntegrationService : ISaltEdgeIntegrationService
{
    private readonly ISaltEdgeHttpClient _client;

    private readonly bool _isProduction;

    public SaltEdgeIntegrationService(ISaltEdgeHttpClient client, bool isProduction)
    {
        _client = client;
        _isProduction = isProduction;
    }


    public async Task<List<SaltEdgeProvider>> FetchProvidersAsync(DateTime? fromDate = null)
    {
        var request = Request.Get("providers").WithQueryParameter("include_fake_providers", !_isProduction);

        if (fromDate is not null)
        {
            request = request.WithQueryParameter("from_date", fromDate);
        }

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error.Message);
        }

        var providers = response.Content?.As<List<SaltEdgeProvider>>();

        return providers ?? new List<SaltEdgeProvider>();
    }
}
