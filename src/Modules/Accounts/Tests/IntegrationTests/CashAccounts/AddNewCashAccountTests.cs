using App.Modules.Accounts.Application.CashAccounts.AddNewCashAccount;
using App.Modules.Accounts.Application.CashAccounts.GetCashAccount;
using App.Modules.Accounts.Application.ViewMetadata.GetViewMetadata;
using App.Modules.Accounts.IntegrationTests.SeedWork;

namespace App.Modules.Accounts.IntegrationTests.CashAccounts;

[TestFixture]
public class AddNewCashAccountTests : TestBase
{
    [Test]
    public async Task AddNewCashAccountCommand_AddsNewCashAccount()
    {
        var command = new AddNewCashAccountCommand(
            Guid.NewGuid(),
            "Cash account",
            "PLN",
            1000);
        var accountId = await AccountsModule.ExecuteCommandAsync(command);

        var createdCashAccount = await AccountsModule.ExecuteQueryAsync(new GetCashAccountQuery(accountId));

        Assert.IsNotNull(createdCashAccount);
        Assert.That(createdCashAccount.UserId, Is.EqualTo(command.UserId));
        Assert.That(createdCashAccount.Title, Is.EqualTo(command.Title));
        Assert.That(createdCashAccount.Balance, Is.EqualTo(command.Balance));
    }

    [Test]
    public async Task AddNewCashAccountCommand_AddsViewMetadata()
    {
        var command = new AddNewCashAccountCommand(
            Guid.NewGuid(),
            "Cash account",
            "PLN",
            1000);
        var accountId = await AccountsModule.ExecuteCommandAsync(command);

        var createdViewMetadata = await AccountsModule.ExecuteQueryAsync(new GetAccountViewMetadataQuery(accountId));

        Assert.IsNotNull(createdViewMetadata);
        Assert.That(createdViewMetadata.AccountId, Is.EqualTo(accountId));
    }
}