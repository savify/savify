using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Accounts;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ResponseContent;
using SaltEdgeConsent = App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ResponseContent.SaltEdgeConsent;

namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge;

public interface ISaltEdgeIntegrationService
{
    public Task<CreateCustomerResponseContent> CreateCustomerAsync(Guid userId);

    public Task<CreateConnectSessionResponseContent> CreateConnectSessionAsync(Guid bankConnectionProcessId,
        string customerId, string providerCode, string returnToUrl);

    public Task<SaltEdgeConnection> FetchConnectionAsync(string connectionId);

    public Task<SaltEdgeConsent> FetchConsentAsync(string consentId, string connectionId);

    public Task<List<SaltEdgeAccount>> FetchAccountsAsync(string connectionId);
}
