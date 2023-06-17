using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.CashAccounts;
using App.Modules.Accounts.Domain.Accounts.CashAccounts.Events;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.UnitTests.Accounts;

[TestFixture]
public class CashAccountsTests : UnitTestBase
{
    [Test]
    public void AddingCashAccount_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var account = CashAccount.AddNew(userId, "Cash", Currency.From("PLN"), 1000);

        var accountAddedDomainEvent = AssertPublishedDomainEvent<CashAccountAddedDomainEvent>(account);
        
        Assert.That(accountAddedDomainEvent.AccountId, Is.EqualTo(account.Id));
        Assert.That(accountAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(accountAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }
}
