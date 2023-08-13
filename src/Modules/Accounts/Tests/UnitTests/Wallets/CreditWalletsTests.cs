using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.CreditWallets;
using App.Modules.Wallets.Domain.Wallets.CreditWallets.Events;
using App.Modules.Wallets.Domain.Users;

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
}
