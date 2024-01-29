using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Rules;
using App.Modules.FinanceTracking.Domain.Wallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.Rules;

namespace App.Modules.FinanceTracking.UnitTests.Wallets.CashWallets;

[TestFixture]
public class CashWalletsTests : UnitTestBase
{
    [Test]
    public void AddingCashWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);

        var walletAddedDomainEvent = AssertPublishedDomainEvent<CashWalletAddedDomainEvent>(wallet);

        Assert.That(walletAddedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }

    [Test]
    public void ChangingTitle_WhenWalletIsRemoved_BreaksCashWalletCannotBeChangedIfWasRemovedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        wallet.Remove();

        AssertBrokenRule<CashWalletCannotBeChangedIfWasRemovedRule>(() =>
        {
            wallet.ChangeTitle("New cash");
        });
    }

    [Test]
    public void ChangingBalance_WhenNewBalanceIsLower_AddsWalletBalanceDecreasedDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);

        wallet.ChangeBalance(100);

        Assert.That(wallet.Balance, Is.EqualTo(100));

        var walletBalanceDecreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceDecreasedDomainEvent>(wallet);
        Assert.That(walletBalanceDecreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceDecreasedDomainEvent.Amount, Is.EqualTo(Money.From(900, Currency.From("PLN"))));
        Assert.That(walletBalanceDecreasedDomainEvent.NewBalance, Is.EqualTo(100));
    }

    [Test]
    public void ChangingBalance_WhenNewBalanceIsHigher_AddsWalletBalanceDecreasedDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);

        wallet.ChangeBalance(2000);

        Assert.That(wallet.Balance, Is.EqualTo(2000));

        var walletBalanceIncreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceIncreasedDomainEvent>(wallet);
        Assert.That(walletBalanceIncreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceIncreasedDomainEvent.Amount, Is.EqualTo(Money.From(1000, Currency.From("PLN"))));
        Assert.That(walletBalanceIncreasedDomainEvent.NewBalance, Is.EqualTo(2000));
    }

    [Test]
    public void ChangingBalance_WhenWalletIsRemoved_BreaksCashWalletCannotBeChangedIfWasRemovedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        wallet.Remove();

        AssertBrokenRule<CashWalletCannotBeChangedIfWasRemovedRule>(() =>
        {
            wallet.ChangeBalance(2000);
        });
    }

    [Test]
    public void RemovingCashWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);

        wallet.Remove();

        var walletRemovedDomainEvent = AssertPublishedDomainEvent<CashWalletRemovedDomainEvent>(wallet);
        Assert.That(walletRemovedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletRemovedDomainEvent.UserId, Is.EqualTo(wallet.UserId));
    }

    [Test]
    public void RemovingCashWallet_WhenWasAlreadyRemoved_BreaksCashWalletCannotBeRemovedMoreThanOnceRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);

        wallet.Remove();

        AssertBrokenRule<CashWalletCannotBeRemovedMoreThanOnceRule>(() =>
        {
            wallet.Remove();
        });
    }

    [Test]
    public void IncreaseBalance_AddsDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        var amount = Money.From(100, Currency.From("PLN"));

        wallet.IncreaseBalance(amount);

        var walletBalanceIncreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceIncreasedDomainEvent>(wallet);
        Assert.That(walletBalanceIncreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceIncreasedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(walletBalanceIncreasedDomainEvent.NewBalance, Is.EqualTo(1100));
    }

    [Test]
    public void IncreaseBalance_WhenCurrenciesDontMatch_BreaksBalanceChangeAmountMustBeInTheWalletCurrencyRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        var amount = Money.From(100, Currency.From("USD"));

        AssertBrokenRule<BalanceChangeAmountMustBeInTheWalletCurrencyRule>(() => wallet.IncreaseBalance(amount));
    }

    [Test]
    public void DecreaseBalance_AddsDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        var amount = Money.From(100, Currency.From("PLN"));

        wallet.DecreaseBalance(amount);

        var walletBalanceDecreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceDecreasedDomainEvent>(wallet);
        Assert.That(walletBalanceDecreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceDecreasedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(walletBalanceDecreasedDomainEvent.NewBalance, Is.EqualTo(900));
    }

    [Test]
    public void DecreaseBalance_WhenCurrenciesDontMatch_BreaksBalanceChangeAmountMustBeInTheWalletCurrencyRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        var amount = Money.From(100, Currency.From("USD"));

        AssertBrokenRule<BalanceChangeAmountMustBeInTheWalletCurrencyRule>(() => wallet.DecreaseBalance(amount));
    }

    [Test]
    public void LoadWalletFromHistory_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        wallet.ClearDomainEvents();

        wallet.IncreaseBalance(Money.From(1000, Currency.From("PLN")));
        wallet.DecreaseBalance(Money.From(100, Currency.From("PLN")));

        var walletHistory = wallet.DomainEvents;

        var loadedWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        loadedWallet.Load(walletHistory);

        Assert.That(loadedWallet.Balance, Is.EqualTo(wallet.Balance));
    }
}
