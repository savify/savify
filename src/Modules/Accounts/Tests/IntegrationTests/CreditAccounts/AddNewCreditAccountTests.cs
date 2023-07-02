using App.Modules.Accounts.Application.CreditAccounts.AddNewCreditAccount;
using App.Modules.Accounts.Application.CreditAccounts.GetCreditAccount;
using App.Modules.Accounts.IntegrationTests.SeedWork;

namespace App.Modules.Accounts.IntegrationTests.CreditAccounts;

[TestFixture]
public class AddNewCreditAccountTests : TestBase
{
    [Test]
    public async Task AddNewCreditAccountCommand_AddsNewCreditAccount()
    {
        var command = new AddNewCreditAccountCommand(
            userId: Guid.NewGuid(),
            title: "Credit account",
            currency: "PLN",
            availableBalance: 500,
            creditLimit: 1000);
        var accountId = await AccountsModule.ExecuteCommandAsync(command);

        var addedCreditAccount = await AccountsModule.ExecuteQueryAsync(new GetCreditAccountQuery(accountId));

        Assert.IsNotNull(addedCreditAccount);
        Assert.That(addedCreditAccount.Id, Is.EqualTo(accountId));
        Assert.That(addedCreditAccount.Title, Is.EqualTo(command.Title));
        Assert.That(addedCreditAccount.AvailableBalance, Is.EqualTo(command.AvailableBalance));
        Assert.That(addedCreditAccount.CreditLimit, Is.EqualTo(command.CreditLimit));
        Assert.That(addedCreditAccount.Currency, Is.EqualTo(command.Currency));

        Assert.IsNotNull(addedCreditAccount.ViewMetadata);
        Assert.That(addedCreditAccount.ViewMetadata.AccountId, Is.EqualTo(accountId));
    }
}
