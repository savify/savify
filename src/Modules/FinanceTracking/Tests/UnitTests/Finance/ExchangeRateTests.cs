using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.UnitTests.Finance;

[TestFixture]
public class ExchangeRateTests : UnitTestBase
{
    [Test]
    public void CreateFor_ReturnsExchangeRateWithRoundedRate()
    {
        var exchangeRate = ExchangeRate.For(Currency.From("USD"), Currency.From("PLN"), 4.1234m);

        Assert.That(exchangeRate.From, Is.EqualTo(Currency.From("USD")));
        Assert.That(exchangeRate.To, Is.EqualTo(Currency.From("PLN")));
        Assert.That(exchangeRate.Rate, Is.EqualTo(4.12m));
    }

    [Test]
    public void CalculateFor_WithRatesToUsdCurrency_ReturnsCalculatedExchangeRate()
    {
        var exchangeRate = ExchangeRate.CalculateFor(Currency.From("USD"), Currency.From("PLN"), 1m, 0.251m);

        Assert.That(exchangeRate.From, Is.EqualTo(Currency.From("USD")));
        Assert.That(exchangeRate.To, Is.EqualTo(Currency.From("PLN")));
        Assert.That(exchangeRate.Rate, Is.EqualTo(3.98m));
    }
}
