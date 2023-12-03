using App.Modules.Transactions.Application.Transactions;
using App.Modules.Transactions.Application.Transactions.AddNewTransaction;
using App.Modules.Transactions.Application.Transactions.GetTransaction;
using App.Modules.Transactions.Application.Transactions.RemoveTransaction;
using App.Modules.Transactions.IntegrationTests.SeedWork;

namespace App.Modules.Transactions.IntegrationTests.Transactions;

[TestFixture]
public class RemoveTransactionTests : TestBase
{
    [Test]
    public async Task RemoveTransactionCommand_Tests()
    {
        var transaction = await AddNewTransactionAsync();

        var command = new RemoveTransactionCommand(transaction.Id);
        await TransactionsModule.ExecuteCommandAsync(command);

        var removedTransaction = await TransactionsModule.ExecuteQueryAsync(new GetTransactionQuery(transaction.Id));

        Assert.That(removedTransaction, Is.Null);
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
