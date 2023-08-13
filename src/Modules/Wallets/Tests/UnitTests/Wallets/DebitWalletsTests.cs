using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets.Events;
using App.Modules.Wallets.Domain.Users;

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
}
