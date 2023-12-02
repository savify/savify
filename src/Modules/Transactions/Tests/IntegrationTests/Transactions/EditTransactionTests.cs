using App.Modules.Transactions.Application.Transactions;
using App.Modules.Transactions.Application.Transactions.AddNewTransaction;
using App.Modules.Transactions.Application.Transactions.EditTransaction;
using App.Modules.Transactions.Application.Transactions.GetTransaction;
using App.Modules.Transactions.Domain.Transactions;
using App.Modules.Transactions.IntegrationTests.SeedWork;
using App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios.Assets;

namespace App.Modules.Transactions.IntegrationTests.Transactions;

[TestFixture]
public class EditTransactionTests : TestBase
{
    [Test]
    public async Task EditTransactionCommand_Tests()
    {
        var transaction = await AddNewTransactionAsync();

        var command = new EditTransactionCommand(
            transaction.Id,
            TransactionTypeDto.Expense,
            new SourceDto("MyMilleniumPLNAccount", 900000, "PLN"),
            new TargetDto("MyMilleniumUSDAccount", 210000, "USD"),
            madeOn: DateTime.UtcNow.AddDays(-20),
            comment: "Accomodation in Slovakia for 20 days",
            tags: new[] { "trip", "skis", "fun", "long vacations" });

        await TransactionsModule.ExecuteCommandAsync(command);

        var editedTransaction = await TransactionsModule.ExecuteQueryAsync(new GetTransactionQuery(transaction.Id));

        Assert.That(editedTransaction, Is.Not.Null);
        Assert.That(editedTransaction.Type, Is.EqualTo(command.Type));
        Assert.That(editedTransaction.MadeOn, Is.EqualTo(command.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(editedTransaction.Comment, Is.EqualTo(command.Comment));
        Assert.That(editedTransaction.Tags, Is.EquivalentTo(command.Tags));

        Assert.That(editedTransaction.Source.SenderAddress, Is.EqualTo(command.Source.SenderAddress));
        Assert.That(editedTransaction.Source.Amount, Is.EqualTo(command.Source.Amount));
        Assert.That(editedTransaction.Source.Currency, Is.EqualTo(command.Source.Currency));

        Assert.That(editedTransaction.Target.RecipientAddress, Is.EqualTo(command.Target.RecipientAddress));
        Assert.That(editedTransaction.Target.Amount, Is.EqualTo(command.Target.Amount));
        Assert.That(editedTransaction.Target.Currency, Is.EqualTo(command.Target.Currency));
    }

    private async Task<TransactionDto> AddNewTransactionAsync()
    {
        var command = new AddNewTransactionCommand(
            TransactionTypeDto.Transfer,
            new SourceDto("MyMilleniumPLNAccount", 450000, "PLN"),
            new TargetDto("MyMilleniumEURAccount", 100000, "EUR"),
            madeOn: DateTime.UtcNow,
            comment: "Accomodation in Slovakia for 10 days",
            tags: new[] { "trip", "skis", "fun", "vacations" });

        var transactionId = await TransactionsModule.ExecuteCommandAsync(command);

        var transaction = await TransactionsModule.ExecuteQueryAsync(new GetTransactionQuery(transactionId));
        return transaction;
    }
}
