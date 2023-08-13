using App.Modules.Wallets.Application.Accounts.CashAccounts.AddNewCashAccount;
using App.Modules.Wallets.Application.Accounts.CashAccounts.GetCashAccount;
using App.Modules.Accounts.IntegrationTests.SeedWork;

namespace App.Modules.Accounts.IntegrationTests.CashAccounts;

[TestFixture]
public class AddNewCashAccountTests : TestBase
{
    [Test]
    public async Task AddNewCashAccountCommand_Tests()
    {
        var command = new AddNewCashAccountCommand(
            Guid.NewGuid(),
            "Cash account",
            "PLN",
            1000);
        var accountId = await WalletsModule.ExecuteCommandAsync(command);

        var addedCashAccount = await WalletsModule.ExecuteQueryAsync(new GetCashAccountQuery(accountId));

        Assert.IsNotNull(addedCashAccount);
        Assert.That(addedCashAccount.UserId, Is.EqualTo(command.UserId));
        Assert.That(addedCashAccount.Title, Is.EqualTo(command.Title));
        Assert.That(addedCashAccount.Balance, Is.EqualTo(command.Balance));

        Assert.IsNotNull(addedCashAccount.ViewMetadata);
        Assert.That(addedCashAccount.ViewMetadata.AccountId, Is.EqualTo(accountId));
    }
}