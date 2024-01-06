using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Rules;

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
    public void EditingCashWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);

        wallet.Edit("New cash", 2000);

        var walletEditedDomainEvent = AssertPublishedDomainEvent<CashWalletEditedDomainEvent>(wallet);
        Assert.That(walletEditedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletEditedDomainEvent.NewBalance, Is.EqualTo(2000));
    }

    [Test]
    public void EditingCashWallet_WhenWalletIsRemoved_BreaksCashWalletCannotBeEditedIfWasRemovedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);

        wallet.Remove();

        AssertBrokenRule<CashWalletCannotBeEditedIfWasRemovedRule>(() =>
        {
            wallet.Edit("New cash", 2000);
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
}
