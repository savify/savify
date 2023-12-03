using App.Modules.Transactions.Domain.Finance;
using App.Modules.Transactions.Domain.Transactions;
using App.Modules.Transactions.Domain.Transactions.Events;

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
            madeOn: new DateTime(year: 2024, month: 01, day: 15),
            comment: "For ski pass.",
            tags: new[] { "Skis", "Vacations", "Love" });

        var domainEvent = AssertPublishedDomainEvent<TransactionAddedDomainEvent>(transaction);
        Assert.Multiple(() =>
        {
            Assert.That(domainEvent.TransactionId, Is.EqualTo(transaction.Id));
            Assert.That(domainEvent.Type, Is.EqualTo(TransactionType.Expense()));
            Assert.That(domainEvent.Source, Is.EqualTo(source));
            Assert.That(domainEvent.Target, Is.EqualTo(target));
        });
    }

    [Test]
    public void EditingTransaction_IsSuccessful()
    {
        var (transaction, type, oldSource, oldTarget) = AddNewTransaction();

        var newType = TransactionType.Transfer();

        var newSource = Source.With(
            Sender.WhoHas("8765-4321-address"),
            Money.From(45000, "PLN"));

        var newTarget = Target.With(
            Recipient.WhoHas("2345-6789-address"),
            Money.From(10000, "EUR"));

        transaction.Edit(
            newType,
            newSource,
            newTarget,
            madeOn: new DateTime(year: 2024, month: 01, day: 20),
            comment: "For ski pass extended.",
            tags: new[] { "Skis", "Extended vacations", "Love" });

        var domainEvent = AssertPublishedDomainEvent<TransactionEditedDomainEvent>(transaction);
        Assert.Multiple(() =>
        {
            Assert.That(domainEvent.TransactionId, Is.EqualTo(transaction.Id));
            Assert.That(domainEvent.Type, Is.EqualTo(newType));
            Assert.That(domainEvent.OldSource, Is.EqualTo(oldSource));
            Assert.That(domainEvent.NewSource, Is.EqualTo(newSource));
            Assert.That(domainEvent.OldTarget, Is.EqualTo(oldTarget));
            Assert.That(domainEvent.NewTarget, Is.EqualTo(newTarget));
        });
    }

    [Test]
    public void RemovingTransaction_IsSuccessful()
    {
        var (transaction, type, source, target) = AddNewTransaction();

        transaction.Remove();

        var domainEvent = AssertPublishedDomainEvent<TransactionRemovedDomainEvent>(transaction);
        Assert.Multiple(() =>
        {
            Assert.That(domainEvent.TransactionId, Is.EqualTo(transaction.Id));
            Assert.That(domainEvent.Type, Is.EqualTo(type));
            Assert.That(domainEvent.Source, Is.EqualTo(source));
            Assert.That(domainEvent.Target, Is.EqualTo(target));
        });
    }

    private static (Transaction transaction, TransactionType type, Source source, Target target) AddNewTransaction()
    {
        var type = TransactionType.Expense();

        var source = Source.With(
                Sender.WhoHas("1234-5678-address"),
                Money.From(42500, "PLN"));

        var target = Target.With(
                Recipient.WhoHas("9876-5432-address"),
                Money.From(10000, "USD"));

        var transaction = Transaction.AddNew(
            type,
            source,
            target,
            madeOn: new DateTime(year: 2024, month: 01, day: 15),
            comment: "For ski pass.",
            tags: new[] { "Skis", "Vacations", "Love" });

        return (transaction, type, source, target);
    }
}
