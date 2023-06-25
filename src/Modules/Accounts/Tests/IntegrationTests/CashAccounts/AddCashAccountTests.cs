using App.Modules.Accounts.Application.CashAccounts.AddNewCashAccount;
using App.Modules.Accounts.Application.CashAccounts.GetCashAccount;
using App.Modules.Accounts.IntegrationTests.SeedWork;

namespace App.Modules.Accounts.IntegrationTests.CashAccounts;

[TestFixture]
public class AddCashAccountTests : TestBase
{
    [Test]
    public async Task AddCashAccountCommand_Test()
    {
        var addCashAccount = new AddNewCashAccountCommand(
            Guid.NewGuid(),
            "Cash account",
            "PLN",
            1000);
        var accountId = await AccountsModule.ExecuteCommandAsync(addCashAccount);

        var createdCashAccount = await AccountsModule.ExecuteQueryAsync(new GetCashAccountQuery(accountId));

        Assert.IsNotNull(createdCashAccount);
        Assert.That(createdCashAccount.UserId, Is.EqualTo(addCashAccount.UserId));
        Assert.That(createdCashAccount.Title, Is.EqualTo(addCashAccount.Title));
        Assert.That(createdCashAccount.Balance, Is.EqualTo(addCashAccount.Balance));
    }
}