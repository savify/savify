using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.UnitTests.Finance;

[TestFixture]
public class TransactionAmountFactoryTests : UnitTestBase
{
    [Test]
    public async Task CreateAsync_WithoutGivenTargetAmount_ButWithDifferentTargetCurrency_GetsExchangeRateFromProvider()
    {
        var sourceCurrency = Currency.From("USD");
        var targetCurrency = Currency.From("PLN");
        var exchangeRate = ExchangeRate.For(sourceCurrency, targetCurrency, 4m);

        var exchangeRatesProvider = Substitute.For<IExchangeRatesProvider>();
        exchangeRatesProvider.GetExchangeRateFor(sourceCurrency, targetCurrency, Arg.Any<DateTime>())
            .Returns(Task.FromResult(exchangeRate));

        var factory = new TransactionAmountFactory(exchangeRatesProvider);

        var amount = await factory.CreateAsync(100, sourceCurrency, null, targetCurrency, DateTime.Now);

        Assert.That(amount.Source, Is.EqualTo(Money.From(100, sourceCurrency)));
        Assert.That(amount.Target, Is.EqualTo(Money.From(400, targetCurrency)));
        Assert.That(amount.ExchangeRate, Is.EqualTo(exchangeRate));
    }

    [Test]
    public async Task CreateAsync_WithGivenTargetAmount_AndDifferentTargetCurrency_CalculatesExchangeRateAndReturnsAmount()
    {
        var sourceCurrency = Currency.From("USD");
        var targetCurrency = Currency.From("PLN");

        var exchangeRatesProvider = Substitute.For<IExchangeRatesProvider>();
        var factory = new TransactionAmountFactory(exchangeRatesProvider);

        var amount = await factory.CreateAsync(100, sourceCurrency, 400, targetCurrency, DateTime.Now);

        Assert.That(amount.Source, Is.EqualTo(Money.From(100, sourceCurrency)));
        Assert.That(amount.Target, Is.EqualTo(Money.From(400, targetCurrency)));
        Assert.That(amount.ExchangeRate, Is.EqualTo(ExchangeRate.For(sourceCurrency, targetCurrency, 4m)));
    }

    [Test]
    public async Task CreateAsync_WithoutGivenTargetAmount_AndWithTheSameTargetCurrency_ReturnsAmountWithEqualSourceAndTargetAmount()
    {
        var currency = Currency.From("USD");

        var exchangeRatesProvider = Substitute.For<IExchangeRatesProvider>();
        var factory = new TransactionAmountFactory(exchangeRatesProvider);

        var amount = await factory.CreateAsync(100, currency, null, currency, DateTime.Now);

        Assert.That(amount.Source, Is.EqualTo(Money.From(100, currency)));
        Assert.That(amount.Target, Is.EqualTo(Money.From(100, currency)));
        Assert.That(amount.ExchangeRate, Is.EqualTo(ExchangeRate.For(currency, currency, 1m)));
    }
}
