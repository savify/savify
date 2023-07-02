using App.Modules.Accounts.Application.Accounts.DebitAccounts.AddNewDebitAccount;
using App.Modules.Accounts.Application.Accounts.DebitAccounts.GetDebitAccount;
using App.Modules.Accounts.IntegrationTests.SeedWork;

namespace App.Modules.Accounts.IntegrationTests.DebitAccounts;

[TestFixture]
public class AddNewDebitAccountTests : TestBase
{
    [Test]
    public async Task AddNewDebitAccountCommand_Tests()
    {
        var command = new AddNewDebitAccountCommand(
            userId: Guid.NewGuid(),
            title: "Debit account",
            currency: "PLN",
            balance: 1000);
        var accountId = await AccountsModule.ExecuteCommandAsync(command);

        var addedDebitAccount = await AccountsModule.ExecuteQueryAsync(new GetDebitAccountQuery(accountId));

        Assert.IsNotNull(addedDebitAccount);
        Assert.That(addedDebitAccount.Id, Is.EqualTo(accountId));
        Assert.That(addedDebitAccount.UserId, Is.EqualTo(command.UserId));
        Assert.That(addedDebitAccount.Title, Is.EqualTo(command.Title));
        Assert.That(addedDebitAccount.Currency, Is.EqualTo(command.Currency));
        Assert.That(addedDebitAccount.Balance, Is.EqualTo(command.Balance));

        Assert.IsNotNull(addedDebitAccount.ViewMetadata);
        Assert.That(addedDebitAccount.ViewMetadata.AccountId, Is.EqualTo(accountId));
    }
}
