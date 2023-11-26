using App.Modules.Transactions.Application.Transactions;
using App.Modules.Transactions.Application.Transactions.AddNewTransaction;
using App.Modules.Transactions.Application.Transactions.GetTransaction;
using App.Modules.Transactions.IntegrationTests.SeedWork;

namespace App.Modules.Transactions.IntegrationTests.Transactions;

[TestFixture]
public class AddNewTransactionTests : TestBase
{
    [Test]
    public async Task AddNewTransactionCommand_Tests()
    {
        var command = new AddNewTransactionCommand(
            TransactionTypeDto.Transfer,
            new SourceDto("MyMilleniumPLNAccount", 450000, "PLN"),
            new TargetDto("MyMilleniumEURAccount", 100000, "EUR"),
            madeOn: DateTime.UtcNow,
            comment: "Accomodation in Slovakia",
            tags: new[] { "trip", "skiis", "fun", "vacations" });

        var transactionId = await TransactionsModule.ExecuteCommandAsync(command);

        var transaction = await TransactionsModule.ExecuteQueryAsync(new GetTransactionQuery(transactionId));

        Assert.IsNotNull(transaction);
        Assert.That(transaction.Type, Is.EqualTo(command.Type));
        Assert.That(transaction.MadeOn, Is.EqualTo(command.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(transaction.Comment, Is.EqualTo(command.Comment));
        Assert.That(transaction.Tags, Is.EquivalentTo(command.Tags));

        Assert.IsNotNull(transaction.Source);
        Assert.That(transaction.Source.SenderAddress, Is.EqualTo(command.Source.SenderAddress));
        Assert.That(transaction.Source.Amount, Is.EqualTo(command.Source.Amount));
        Assert.That(transaction.Source.Currency, Is.EqualTo(command.Source.Currency));

        Assert.IsNotNull(transaction.Target);
        Assert.That(transaction.Target.RecipientAddress, Is.EqualTo(command.Target.RecipientAddress));
        Assert.That(transaction.Target.Amount, Is.EqualTo(command.Target.Amount));
        Assert.That(transaction.Target.Currency, Is.EqualTo(command.Target.Currency));
    }
}
