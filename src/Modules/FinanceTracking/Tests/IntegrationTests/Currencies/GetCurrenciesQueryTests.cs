using App.Modules.FinanceTracking.Application.Currencies.GetCurrencies;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Currencies;

[TestFixture]
public class GetCurrenciesQueryTests : TestBase
{
    [Test]
    public async Task GetCurrenciesQuery_ReturnsSupportedCurrencies()
    {
        SaltEdgeHttpClientMocker.MockFetchCurrenciesSuccessfulResponse();

        var currencies = await FinanceTrackingModule.ExecuteQueryAsync(new GetCurrenciesQuery());

        Assert.That(currencies, Contains.Item("USD"));
        Assert.That(currencies, Contains.Item("EUR"));
        Assert.That(currencies, Contains.Item("PLN"));
        Assert.That(currencies, Has.No.Member("RUB"));
    }
}
