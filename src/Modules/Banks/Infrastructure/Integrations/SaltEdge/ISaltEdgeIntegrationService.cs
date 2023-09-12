using App.Modules.Banks.Infrastructure.Integrations.SaltEdge.Providers;

namespace App.Modules.Banks.Infrastructure.Integrations.SaltEdge;

public interface ISaltEdgeIntegrationService
{
    public Task<List<SaltEdgeProvider>> FetchProvidersAsync(DateTime? fromDate);
}
