using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.DebitAccounts;
using App.Modules.Accounts.Domain.Accounts.DebitAccounts.Events;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.UnitTests.Accounts;

[TestFixture]
public class DebitAccountsTests : UnitTestBase
{
    [Test]
    public void AddingDebitAccount_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var account = DebitAccount.AddNew(userId, "Debit", Currency.From("PLN"), 1000);

        var accountAddedDomainEvent = AssertPublishedDomainEvent<DebitAccountAddedDomainEvent>(account);

        Assert.That(accountAddedDomainEvent.AccountId, Is.EqualTo(account.Id));
        Assert.That(accountAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(accountAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }
}
