using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets.Events;

namespace App.Modules.Wallets.UnitTests.Wallets;

[TestFixture]
public class DebitWalletsTests : UnitTestBase
{
    [Test]
    public void AddingDebitWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);

        var walletAddedDomainEvent = AssertPublishedDomainEvent<DebitWalletAddedDomainEvent>(wallet);

        Assert.That(walletAddedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }
    
    [Test]
    public void EditingDebitWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        
        wallet.Edit("New cash", new Currency("GBP"), 2000);

        var walletEditedDomainEvent = AssertPublishedDomainEvent<DebitWalletEditedDomainEvent>(wallet);
        Assert.That(walletEditedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletEditedDomainEvent.NewCurrency, Is.EqualTo(new Currency("GBP")));
        Assert.That(walletEditedDomainEvent.NewBalance, Is.EqualTo(2000));
    }
}
