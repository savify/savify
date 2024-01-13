using App.Integrations.SaltEdge.Client;
using App.Integrations.SaltEdge.Requests;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Accounts;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Currencies;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ExchangeRates;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.RequestContent;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ResponseContent;
using SaltEdgeConsent = App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ResponseContent.SaltEdgeConsent;

namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge;

public class SaltEdgeIntegrationService(ISaltEdgeHttpClient client) : ISaltEdgeIntegrationService, ISaltEdgeCurrenciesProvider, ISaltEdgeExchangeRatesProvider
{
    public async Task<CreateCustomerResponseContent> CreateCustomerAsync(Guid userId)
    {
        var request = Request.Post("customers")
            .WithContent(new CreateCustomerRequestContent(userId.ToString()));
        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error!.Message);
        }

        return response.Content?.As<CreateCustomerResponseContent>()!;
    }

    public async Task<CreateConnectSessionResponseContent> CreateConnectSessionAsync(Guid bankConnectionProcessId, string customerId, string providerCode, string returnToUrl)
    {
        var request = Request.Post("connect_sessions/create")
            .WithContent(new CreateConnectSessionRequestContent(
                customerId,
                providerCode,
                Consent.Default,
                new Attempt(bankConnectionProcessId, returnToUrl)));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error!.Message);
        }

        return response.Content?.As<CreateConnectSessionResponseContent>()!;
    }

    public async Task<SaltEdgeConnection> FetchConnectionAsync(string connectionId)
    {
        var request = Request.Get($"connections/{connectionId}");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error!.Message);
        }

        var connection = response.Content?.As<SaltEdgeConnection>();

        if (connection is null)
        {
            throw new SaltEdgeIntegrationException($"Connection with ID '{connectionId}' was not found");
        }

        return connection;
    }

    public async Task<SaltEdgeConsent> FetchConsentAsync(string consentId, string connectionId)
    {
        var request = Request.Get($"consents/{consentId}").WithQueryParameter("connection_id", connectionId);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error!.Message);
        }

        var consent = response.Content?.As<SaltEdgeConsent>();

        if (consent is null)
        {
            throw new SaltEdgeIntegrationException($"Consent with ID '{consentId}' for connection with ID '{connectionId}' was not found");
        }

        return consent;
    }

    public async Task<List<SaltEdgeAccount>> FetchAccountsAsync(string connectionId)
    {
        var request = Request.Get("accounts").WithQueryParameter("connection_id", connectionId);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error!.Message);
        }

        var accounts = response.Content?.As<List<SaltEdgeAccount>>();

        if (accounts is null)
        {
            throw new SaltEdgeIntegrationException($"Accounts for connection ID '{connectionId}' were not found");
        }

        return accounts;
    }

    public async Task<IEnumerable<CurrencyDto>> FetchCurrenciesAsync()
    {
        var request = Request.Get("currencies");
        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error!.Message);
        }

        var currencies = response.Content?.As<List<CurrencyDto>>();

        if (currencies is null)
        {
            throw new SaltEdgeIntegrationException("Currencies were not found");
        }

        return currencies.AsEnumerable();
    }

    public async Task<IEnumerable<ExchangeRateDto>> FetchExchangeRatesAsync(DateTime? date = null)
    {
        var request = Request.Get("rates");

        if (date is not null)
        {
            request = (GetRequest)request.WithQueryParameter("date", date);
        }

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error!.Message);
        }

        var exchangeRates = response.Content?.As<List<ExchangeRateDto>>();

        if (exchangeRates is null)
        {
            throw new SaltEdgeIntegrationException("Exchange rates were not found");
        }

        return exchangeRates.AsEnumerable();
    }
}
