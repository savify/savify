using App.Modules.FinanceTracking.IntegrationTests.BankConnectionProcessing;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace App.Modules.FinanceTracking.IntegrationTests.SeedData;

public class SaltEdgeHttpClientMocker
{
    private readonly WireMockServer _wireMock = WireMockServer.Start();

    public string BaseUrl => _wireMock.Url!;

    public void StopWireMockServer()
    {
        _wireMock.Stop();
    }

    public void MockCreateCustomerSuccessfulResponse()
    {
        _wireMock.Given(
                Request.Create()
                    .WithPath("/customers")
                    .WithBodyAsJson(new { data = new { identifier = BankConnectionProcessingData.UserId.ToString() } })
                    .UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(201)
                    .WithBodyAsJson(new
                    {
                        data = new
                        {
                            id = BankConnectionProcessingData.ExternalCustomerId,
                            identifier = BankConnectionProcessingData.UserId.ToString(),
                            secret = "secret"
                        }
                    }));
    }

    public void MockCreateConnectSessionSuccessfulResponse()
    {
        _wireMock.Given(
                Request.Create()
                    .WithPath("/connect_sessions/create")
                    .WithBody(new JsonPartialMatcher(new
                    {
                        data = new
                        {
                            customer_id = BankConnectionProcessingData.ExternalCustomerId,
                            provider_code = BankConnectionProcessingData.ExternalProviderCode,
                            attempt = new
                            {
                                locale = BankConnectionProcessingData.UserLanguage
                            }
                        }
                    }))
                    .UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(201)
                    .WithBodyAsJson(new
                    {
                        data = new
                        {
                            expires_at = "2023-08-22T07:58:14Z",
                            connect_url = BankConnectionProcessingData.ExpectedRedirectUrl
                        }
                    }));
    }

    public void MockFetchConnectionSuccessfulResponse()
    {
        _wireMock.Given(
                Request.Create()
                    .WithPath($"/connections/{BankConnectionProcessingData.ExternalConnectionId}")
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        data = new
                        {
                            id = BankConnectionProcessingData.ExternalConnectionId,
                            provider_code = BankConnectionProcessingData.ExternalProviderCode,
                            country_code = BankConnectionProcessingData.CountryCode,
                            last_consent_id = BankConnectionProcessingData.ExternalConsentId,
                            customer_id = BankConnectionProcessingData.ExternalCustomerId,
                            status = "active"
                        }
                    }));
    }

    public void MockFetchConsentSuccessfulResponse()
    {
        _wireMock.Given(
                Request.Create()
                    .WithPath($"/consents/{BankConnectionProcessingData.ExternalConsentId}")
                    .WithParam("connection_id", BankConnectionProcessingData.ExternalConnectionId)
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        data = new
                        {
                            id = BankConnectionProcessingData.ExternalConsentId,
                            expires_at = DateTime.UtcNow.ToString("O")
                        }
                    }));
    }

    public void MockFetchAccountsSuccessfulResponse(bool hasMultipleAccounts = true)
    {
        var singleAccountJson = new
        {
            data = new object[]
            {
                new
                {
                    id = BankConnectionProcessingData.ExternalUsdAccountId,
                    name = "USD Account",
                    nature = "account",
                    balance = BankConnectionProcessingData.ExternalUsdAccountBalance,
                    currency_code = BankConnectionProcessingData.ExternalUsdAccountCurrency,
                }
            }
        };

        var multipleAccountsJson = new
        {
            data = new object[]
            {
                new
                {
                    id = BankConnectionProcessingData.ExternalUsdAccountId,
                    name = "USD Account",
                    nature = "account",
                    balance = BankConnectionProcessingData.ExternalUsdAccountBalance,
                    currency_code = BankConnectionProcessingData.ExternalUsdAccountCurrency,
                },
                new
                {
                    id = BankConnectionProcessingData.ExternalPlnAccountId,
                    name = "PLN Account",
                    nature = "account",
                    balance = BankConnectionProcessingData.ExternalPlnAccountBalance,
                    currency_code = BankConnectionProcessingData.ExternalPlnAccountCurrency,
                }
            }
        };

        _wireMock.Given(
                Request.Create()
                    .WithPath("/accounts")
                    .WithParam("connection_id", BankConnectionProcessingData.ExternalConnectionId)
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(hasMultipleAccounts ? multipleAccountsJson : singleAccountJson));
    }

    public void MockFetchCurrenciesSuccessfulResponse()
    {
        var currenciesJson = new
        {
            data = new object[]
            {
                new
                {
                    code = "USD",
                    name = "United States Dollar",
                },
                new
                {
                    code = "EUR",
                    name = "Euro",
                },
                new
                {
                    code = "PLN",
                    name = "Polish Zloty",
                },
                new
                {
                    code = "RUB",
                    name = "A piece of shit",
                },
            }
        };

        _wireMock.Given(
                Request.Create()
                    .WithPath("/currencies")
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(currenciesJson));
    }

    public void MockFetchExchangeRatesSuccessfulResponse()
    {
        var currenciesJson = new
        {
            data = new object[]
            {
                new
                {
                    currency_code = "USD",
                    rate = 1m,
                },
                new
                {
                    currency_code = "EUR",
                    rate = 1.20m,
                },
                new
                {
                    currency_code = "PLN",
                    rate = 0.25m,
                },
            }
        };

        _wireMock.Given(
                Request.Create()
                    .WithPath("/rates")
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(currenciesJson));
    }
}
