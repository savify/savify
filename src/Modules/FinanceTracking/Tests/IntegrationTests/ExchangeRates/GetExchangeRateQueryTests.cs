using App.Modules.FinanceTracking.Application.ExchangeRates.GetExchangeRate;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.ExchangeRates;

[TestFixture]
public class GetExchangeRateQueryTests : TestBase
{
    [Test]
    public async Task GetExchangeRateQuery_ReturnsCurrentExchangeRate_ForGivenCurrencies()
    {
        // available currencies: USD, PLN, EUR
        SaltEdgeHttpClientMocker.MockFetchCurrenciesSuccessfulResponse();
        // EUR -> USD: 1.2; PLN -> USD: 0.25
        SaltEdgeHttpClientMocker.MockFetchExchangeRatesSuccessfulResponse();

        var exchangeRate = await FinanceTrackingModule.ExecuteQueryAsync(new GetExchangeRateQuery("EUR", "PLN"));

        Assert.That(exchangeRate, Is.EqualTo(4.80));
    }
}
