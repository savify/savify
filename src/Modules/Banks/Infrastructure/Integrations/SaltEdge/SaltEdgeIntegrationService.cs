using App.Integrations.SaltEdge.Client;
using App.Integrations.SaltEdge.Requests;
using App.Modules.Banks.Infrastructure.Integrations.SaltEdge.Providers;

namespace App.Modules.Banks.Infrastructure.Integrations.SaltEdge;

public class SaltEdgeIntegrationService(ISaltEdgeHttpClient client, bool isProduction) : ISaltEdgeIntegrationService
{
    public async Task<List<SaltEdgeProvider>> FetchProvidersAsync(DateTime? fromDate = null)
    {
        var request = Request.Get("providers").WithQueryParameter("include_fake_providers", !isProduction);

        if (fromDate is not null)
        {
            request = request.WithQueryParameter("from_date", fromDate);
        }

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error!.Message);
        }

        var providers = response.Content?.As<List<SaltEdgeProvider>>();

        return providers ?? new List<SaltEdgeProvider>();
    }
}
