using App.Modules.Wallets.Domain.Accounts;
using App.Modules.Wallets.Domain.Accounts.DebitAccounts;
using App.Modules.Wallets.Domain.Accounts.DebitAccounts.Events;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Accounts.UnitTests.Accounts;

[TestFixture]
public class DebitAccountsTests : UnitTestBase
{
    [Test]
    public void AddingDebitAccount_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var account = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);

        var accountAddedDomainEvent = AssertPublishedDomainEvent<DebitWalletAddedDomainEvent>(account);

        Assert.That(accountAddedDomainEvent.WalletId, Is.EqualTo(account.Id));
        Assert.That(accountAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(accountAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }
}
