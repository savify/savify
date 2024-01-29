using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets.Rules;
using App.Modules.FinanceTracking.Domain.Wallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.Rules;

namespace App.Modules.FinanceTracking.UnitTests.Wallets.CreditWallets;

[TestFixture]
public class CreditWalletsTests : UnitTestBase
{
    [Test]
    public void AddCreditWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), creditLimit: 1000, initialAvailableBalance: 1000);

        var walletAddedDomainEvent = AssertPublishedDomainEvent<CreditWalletAddedDomainEvent>(wallet);

        Assert.That(walletAddedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }

    [Test]
    public void ChangingTitle_WhenWalletIsRemoved_BreaksCreditWalletCannotBeChangedIfWasRemovedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);
        wallet.Remove();

        AssertBrokenRule<CreditWalletCannotBeChangedIfWasRemovedRule>(() =>
        {
            wallet.ChangeTitle("New debit");
        });
    }

    [Test]
    public void ChangingCreditLimit_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);

        wallet.ChangeCreditLimit(2000);

        Assert.That(wallet.CreditLimit, Is.EqualTo(2000));
    }

    [Test]
    public void ChangingCreditLimit_WhenWalletIsRemoved_BreaksCreditWalletCannotBeChangedIfWasRemovedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);
        wallet.Remove();

        AssertBrokenRule<CreditWalletCannotBeChangedIfWasRemovedRule>(() =>
        {
            wallet.ChangeCreditLimit(2000);
        });
    }

    [Test]
    public void ChangingAvailableBalance_WhenNewBalanceIsLower_AddsWalletBalanceDecreasedDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);

        wallet.ChangeAvailableBalance(100);

        Assert.That(wallet.AvailableBalance, Is.EqualTo(100));

        var walletBalanceDecreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceDecreasedDomainEvent>(wallet);
        Assert.That(walletBalanceDecreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceDecreasedDomainEvent.Amount, Is.EqualTo(Money.From(900, Currency.From("PLN"))));
        Assert.That(walletBalanceDecreasedDomainEvent.NewBalance, Is.EqualTo(100));
    }

    [Test]
    public void ChangingAvailableBalance_WhenNewBalanceIsHigher_AddsWalletBalanceDecreasedDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);

        wallet.ChangeAvailableBalance(2000);

        Assert.That(wallet.AvailableBalance, Is.EqualTo(2000));

        var walletBalanceIncreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceIncreasedDomainEvent>(wallet);
        Assert.That(walletBalanceIncreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceIncreasedDomainEvent.Amount, Is.EqualTo(Money.From(1000, Currency.From("PLN"))));
        Assert.That(walletBalanceIncreasedDomainEvent.NewBalance, Is.EqualTo(2000));
    }

    [Test]
    public void ChangingAvailableBalance_WhenWalletIsRemoved_BreaksCreditWalletCannotBeChangedIfWasRemovedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);
        wallet.Remove();

        AssertBrokenRule<CreditWalletCannotBeChangedIfWasRemovedRule>(() =>
        {
            wallet.ChangeAvailableBalance(2000);
        });
    }

    [Test]
    public void RemovingCreditWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);

        wallet.Remove();

        var walletRemovedDomainEvent = AssertPublishedDomainEvent<CreditWalletRemovedDomainEvent>(wallet);
        Assert.That(walletRemovedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletRemovedDomainEvent.UserId, Is.EqualTo(wallet.UserId));
    }

    [Test]
    public void RemovingCreditWallet_WhenWasAlreadyRemoved_BreaksCreditWalletCannotBeRemovedMoreThanOnceRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);

        wallet.Remove();

        AssertBrokenRule<CreditWalletCannotBeRemovedMoreThanOnceRule>(() =>
        {
            wallet.Remove();
        });
    }

    [Test]
    public void IncreaseAvailableBalance_AddsDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);
        var amount = Money.From(100, Currency.From("PLN"));

        wallet.IncreaseBalance(amount);

        var walletBalanceIncreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceIncreasedDomainEvent>(wallet);
        Assert.That(walletBalanceIncreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceIncreasedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(walletBalanceIncreasedDomainEvent.NewBalance, Is.EqualTo(1100));
    }

    [Test]
    public void IncreaseAvailableBalance_WhenCurrenciesDontMatch_BreaksBalanceChangeAmountMustBeInTheWalletCurrencyRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);
        var amount = Money.From(100, Currency.From("USD"));

        AssertBrokenRule<BalanceChangeAmountMustBeInTheWalletCurrencyRule>(() => wallet.IncreaseBalance(amount));
    }

    [Test]
    public void DecreaseAvailableBalance_AddsDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);
        var amount = Money.From(100, Currency.From("PLN"));

        wallet.DecreaseBalance(amount);

        var walletBalanceDecreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceDecreasedDomainEvent>(wallet);
        Assert.That(walletBalanceDecreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceDecreasedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(walletBalanceDecreasedDomainEvent.NewBalance, Is.EqualTo(900));
    }

    [Test]
    public void DecreaseAvailableBalance_WhenCurrenciesDontMatch_BreaksBalanceChangeAmountMustBeInTheWalletCurrencyRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);
        var amount = Money.From(100, Currency.From("USD"));

        AssertBrokenRule<BalanceChangeAmountMustBeInTheWalletCurrencyRule>(() => wallet.DecreaseBalance(amount));
    }

    [Test]
    public void LoadWalletFromHistory_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);
        wallet.ClearDomainEvents();

        wallet.IncreaseBalance(Money.From(1000, Currency.From("PLN")));
        wallet.DecreaseBalance(Money.From(100, Currency.From("PLN")));

        var walletHistory = wallet.DomainEvents;

        var loadedWallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), 1000, 1000);
        loadedWallet.Load(walletHistory);

        Assert.That(loadedWallet.AvailableBalance, Is.EqualTo(wallet.AvailableBalance));
    }
}
