﻿using App.Modules.Wallets.Domain.Accounts;
using App.Modules.Wallets.Domain.Accounts.CreditAccounts;
using App.Modules.Wallets.Domain.Accounts.CreditAccounts.Events;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Accounts.UnitTests.Accounts;

[TestFixture]
public class CreditAccountsTests : UnitTestBase
{
    [Test]
    public void AddCreditAccount_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var account = CreditAccount.AddNew(userId, "Credit", Currency.From("PLN"), creditLimit: 1000, availableBalance: 1000);

        var accountAddedDomainEvent = AssertPublishedDomainEvent<CreditAccountAddedDomainEvent>(account);

        Assert.That(accountAddedDomainEvent.WalletId, Is.EqualTo(account.Id));
        Assert.That(accountAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(accountAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }
}
