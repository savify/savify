using App.Modules.Banks.Infrastructure.Integrations.SaltEdge.Providers;

namespace App.Modules.Banks.Infrastructure.Integrations.SaltEdge;

public interface ISaltEdgeIntegrationService
{
    public Task<IEnumerable<SaltEdgeProvider>> FetchProvidersAsync(DateTime? fromDate = null);
}
