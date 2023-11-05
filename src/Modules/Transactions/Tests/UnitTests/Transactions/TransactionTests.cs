using App.Modules.Transactions.Domain.Finance;
using App.Modules.Transactions.Domain.Transactions;

namespace App.Modules.Transactions.UnitTests.Transactions;

[TestFixture]
public class TransactionTests : UnitTestBase
{
    [Test]
    public void AddingTransaction_IsSuccessful()
    {
        var source = Source.With(
                Sender.WhoHas("1234-5678-address"),
                Money.From(42500, "PLN"));

        var target = Target.With(
                Recipient.WhoHas("9876-5432-address"),
                Money.From(10000, "USD"));

        var transaction = Transaction.AddNew(
            TransactionType.Expense(),
            source,
            target,
            comment: "For ski pass.",
            tags: new[] { "Skis", "Vacations", "Love" });

        var domainEvent = AssertPublishedDomainEvent<TransactionAddedDomainEvent>(transaction);
        Assert.That(domainEvent.TransactionId, Is.EqualTo(transaction.Id));
        Assert.That(domainEvent.Type, Is.EqualTo(TransactionType.Expense()));
        Assert.That(domainEvent.Source, Is.EqualTo(source));
        Assert.That(domainEvent.Target, Is.EqualTo(target));
    }
}
