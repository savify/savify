using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.CreditWallets;
using App.Modules.Wallets.Domain.Wallets.CreditWallets.Events;

namespace App.Modules.Wallets.UnitTests.Wallets;

[TestFixture]
public class CreditWalletsTests : UnitTestBase
{
    [Test]
    public void AddCreditWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), creditLimit: 1000, availableBalance: 1000);

        var walletAddedDomainEvent = AssertPublishedDomainEvent<CreditWalletAddedDomainEvent>(wallet);

        Assert.That(walletAddedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }

    [Test]
    public void EditingCreditWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Credit", Currency.From("PLN"), creditLimit: 1000, availableBalance: 1000);

        wallet.Edit("New credit", new Currency("GBP"), 2000, 2000);

        var walletEditedDomainEvent = AssertPublishedDomainEvent<CreditWalletEditedDomainEvent>(wallet);
        Assert.That(walletEditedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletEditedDomainEvent.NewCurrency, Is.EqualTo(new Currency("GBP")));
        Assert.That(walletEditedDomainEvent.NewAvailableBalance, Is.EqualTo(2000));
        Assert.That(walletEditedDomainEvent.NewCreditLimit, Is.EqualTo(2000));
    }
}
