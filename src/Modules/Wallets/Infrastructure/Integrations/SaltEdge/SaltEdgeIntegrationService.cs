using App.Integrations.SaltEdge.Client;
using App.Integrations.SaltEdge.Requests;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Accounts;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Connections;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.RequestContent;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent;
using Consent = App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent.Consent;

namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;

public class SaltEdgeIntegrationService
{
    private readonly ISaltEdgeHttpClient _client;

    public SaltEdgeIntegrationService(ISaltEdgeHttpClient client)
    {
        _client = client;
    }

    public async Task<CreateCustomerResponseContent> CreateCustomer(Guid userId)
    {
        var request = Request.Post("/customers")
            .WithContent(new CreateCustomerRequestContent(userId.ToString()));
        var response = await _client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error.Message);
        }

        return response.Content?.As<CreateCustomerResponseContent>()!;
    }

    public async Task<CreateConnectSessionResponseContent> CreateConnectSession(Guid bankConnectionProcessId, string customerId, string providerCode, string returnToUrl)
    {
        var request = Request.Post("/connect_sessions/create")
            .WithContent(new CreateConnectSessionRequestContent(
                customerId,
                providerCode,
                RequestContent.Consent.Default,
                new Attempt(bankConnectionProcessId, returnToUrl)));

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error.Message);
        }

        return response.Content?.As<CreateConnectSessionResponseContent>()!;
    }

    public async Task<SaltEdgeConnection> FetchConnection(string connectionId)
    {
        var request = Request.Get($"/connections/{connectionId}");

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error.Message);
        }

        var connection = response.Content?.As<SaltEdgeConnection>();

        if (connection is null)
        {
            throw new SaltEdgeIntegrationException($"Connection with ID '{connectionId}' was not found");
        }

        return connection;
    }

    public async Task<Consent> FetchConsent(string consentId, string connectionId)
    {
        var request = Request.Get($"/consents/{consentId}").WithQueryParameter("connection_id", connectionId);

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error.Message);
        }

        var consent = response.Content?.As<Consent>();

        if (consent is null)
        {
            throw new SaltEdgeIntegrationException($"Consent with ID '{consentId}' for connection with ID '{connectionId}' was not found");
        }

        return consent;
    }

    public async Task<List<SaltEdgeAccount>> FetchAccounts(string connectionId)
    {
        var request = Request.Get("/accounts").WithQueryParameter("connection_id", connectionId);

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error.Message);
        }

        var accounts = response.Content?.As<List<SaltEdgeAccount>>();

        if (accounts is null)
        {
            throw new SaltEdgeIntegrationException($"Accounts for connection ID '{connectionId}' were not found");
        }

        return accounts;
    }
}
