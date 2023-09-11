using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Accounts;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Connections;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent;
using Consent = App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent.Consent;

namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;

public interface ISaltEdgeIntegrationService
{
    public Task<CreateCustomerResponseContent> CreateCustomerAsync(Guid userId);

    public Task<CreateConnectSessionResponseContent> CreateConnectSessionAsync(Guid bankConnectionProcessId,
        string customerId, string providerCode, string returnToUrl);

    public Task<SaltEdgeConnection> FetchConnectionAsync(string connectionId);

    public Task<Consent> FetchConsentAsync(string consentId, string connectionId);

    public Task<List<SaltEdgeAccount>> FetchAccountsAsync(string connectionId);
}
