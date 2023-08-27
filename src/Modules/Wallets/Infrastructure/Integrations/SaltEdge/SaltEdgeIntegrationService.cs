using App.Integrations.SaltEdge.Client;
using App.Integrations.SaltEdge.Requests;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.RequestContent;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent;

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

    public async Task<CreateConnectSessionResponseContent> CreateConnectSession(string customerId, string providerCode, string returnToUrl)
    {
        var request = Request.Post("/connect_sessions/create")
            .WithContent(new CreateConnectSessionRequestContent(
                customerId,
                providerCode,
                Consent.Default,
                new Attempt(returnToUrl)));

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error.Message);
        }

        return response.Content?.As<CreateConnectSessionResponseContent>()!;
    }

    public async Task<Connection> FetchConnectionFor(string customerId, string providerCode)
    {
        var request = Request.Get("/connections").WithQueryParameter("customer_id", customerId);

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error.Message);
        }

        var connection = response.Content?.As<List<Connection>>()?.FirstOrDefault(c => c.ProviderCode == providerCode);

        if (connection is null)
        {
            throw new SaltEdgeIntegrationException(string.Format("Connection for customer ID '{0}' and provider code '{1}' was not found", customerId, providerCode));
        }

        return connection;
    }
}
