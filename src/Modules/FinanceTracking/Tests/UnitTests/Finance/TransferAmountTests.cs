using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;

namespace App.Modules.FinanceTracking.UnitTests.Finance;

[TestFixture]
public class TransferAmountTests : UnitTestBase
{
    [Test]
    public void CreateFrom_SourceAndTargetAmount_ReturnsTransactionAmountWithCalculatedExchangeRate()
    {
        var source = Money.From(100, Currency.From("PLN"));
        var target = Money.From(25, Currency.From("USD"));

        var amount = TransferAmount.From(source, target);

        Assert.That(amount.Source, Is.EqualTo(source));
        Assert.That(amount.Target, Is.EqualTo(target));
        Assert.That(amount.ExchangeRate.From, Is.EqualTo(source.Currency));
        Assert.That(amount.ExchangeRate.To, Is.EqualTo(target.Currency));
        Assert.That(amount.ExchangeRate.Rate, Is.EqualTo(25 / 100m));
    }

    [Test]
    public void CreateFrom_SourceAmount_AndExchangeRate_ReturnsTransactionAmountWithCalculatedTargetAmount()
    {
        var source = Money.From(100, Currency.From("PLN"));
        var exchangeRate = ExchangeRate.For(source.Currency, Currency.From("USD"), 0.25m);

        var amount = TransferAmount.From(source, exchangeRate);

        Assert.That(amount.Source, Is.EqualTo(source));
        Assert.That(amount.Target, Is.EqualTo(Money.From(25, Currency.From("USD"))));
        Assert.That(amount.ExchangeRate, Is.EqualTo(exchangeRate));
    }

    [Test]
    public void CreateFrom_SourceAmount_ReturnsTransactionAmountWithExchangeRate()
    {
        var source = Money.From(100, Currency.From("PLN"));

        var amount = TransferAmount.From(source);

        Assert.That(amount.Source, Is.EqualTo(source));
        Assert.That(amount.Target, Is.EqualTo(source));
        Assert.That(amount.ExchangeRate.From, Is.EqualTo(source.Currency));
        Assert.That(amount.ExchangeRate.To, Is.EqualTo(source.Currency));
        Assert.That(amount.ExchangeRate.Rate, Is.EqualTo(1m));
    }
}
