using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Transfers.Rules;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.UnitTests.Transfers;

[TestFixture]
public class TransferTests : UnitTestBase
{
    [Test]
    public void AddingNewTransfer_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = new WalletId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var transfer = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, comment, tags);

        var transferAddedDomainEvent = AssertPublishedDomainEvent<TransferAddedDomainEvent>(transfer);

        Assert.That(transferAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(transferAddedDomainEvent.SourceWalletId, Is.EqualTo(sourceWalletId));
        Assert.That(transferAddedDomainEvent.TargetWalletId, Is.EqualTo(targetWalletId));
        Assert.That(transferAddedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(transferAddedDomainEvent.Tags, Is.EquivalentTo(tags));
    }

    [Test]
    public void AddingNewTranfer_WhenSourceAndTargetWalletIdsAreTheSame_BreaksTransferSourceAndTargetWalletsMustBeDifferentRule()
    {
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = sourceWalletId;

        var userId = new UserId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        AssertBrokenRule<TransferSourceAndTargetWalletsMustBeDifferentRule>(() =>
        {
            var _ = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, comment, tags);
        });
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public void AddingNewTransfer_WhenTransferAmountIsLessOrEqualToZero_BreaksTransferAmountMustBeBiggerThanZeroRule(int amountValue)
    {
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = new WalletId(Guid.NewGuid());

        var userId = new UserId(Guid.NewGuid());
        var amount = Money.From(amountValue, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        AssertBrokenRule<TransferAmountMustBeBiggerThanZeroRule>(() =>
        {
            var _ = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, comment, tags);
        });
    }

    [Test]
    public void EditingTransfer_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = new WalletId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var transfer = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, comment, tags);

        var newSourceWalletId = new WalletId(Guid.NewGuid());
        var newTargetWalletId = new WalletId(Guid.NewGuid());
        var newAmount = Money.From(500, "USD");
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Eddited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        transfer.Edit(userId, newSourceWalletId, newTargetWalletId, newAmount, newMadeOn, newComment, newTags);

        var transferEdditedDomainEvent = AssertPublishedDomainEvent<TransferEditedDomainEvent>(transfer);

        Assert.That(transferEdditedDomainEvent.OldSourceWalletId, Is.EqualTo(sourceWalletId));
        Assert.That(transferEdditedDomainEvent.NewSourceWalletId, Is.EqualTo(newSourceWalletId));

        Assert.That(transferEdditedDomainEvent.OldTargetWalletId, Is.EqualTo(targetWalletId));
        Assert.That(transferEdditedDomainEvent.NewTargetWalletId, Is.EqualTo(newTargetWalletId));

        Assert.That(transferEdditedDomainEvent.OldAmount, Is.EqualTo(amount));
        Assert.That(transferEdditedDomainEvent.NewAmount, Is.EqualTo(newAmount));

        Assert.That(transferEdditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(transferEdditedDomainEvent.Tags, Is.EqualTo(newTags));
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public void EdditingTransfer_WhenNewAmountIsLessOrEqualToZero_BreaksTransferAmountMustBeBiggerThanZeroRule(int newAmountValue)
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = new WalletId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var transfer = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, comment, tags);

        var newSourceWalletId = new WalletId(Guid.NewGuid());
        var newTargetWalletId = new WalletId(Guid.NewGuid());
        var newAmount = Money.From(newAmountValue, "USD");
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Eddited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        AssertBrokenRule<TransferAmountMustBeBiggerThanZeroRule>(() =>
        {
            transfer.Edit(userId, newSourceWalletId, newTargetWalletId, newAmount, newMadeOn, newComment, newTags);
        });
    }

    [Test]
    public void EdditingTransfer_WhenNewSourceAndTargetWalletIdsAreTheSame_BreaksTransferSourceAndTargetWalletsMustBeDifferentRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = new WalletId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var categoryId = new CategoryId(Guid.NewGuid());
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var transfer = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, comment, tags);

        var newSourceWalletId = new WalletId(Guid.NewGuid());
        var newTargetWalletId = newSourceWalletId;
        var newAmount = Money.From(500, "USD");
        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Eddited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        AssertBrokenRule<TransferSourceAndTargetWalletsMustBeDifferentRule>(() =>
        {
            transfer.Edit(userId, newSourceWalletId, newTargetWalletId, newAmount, newMadeOn, newComment, newTags);
        });
    }

    [Test]
    public void RemovingTransfer_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = new WalletId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var transfer = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, comment, tags);

        transfer.Remove();

        var transferRemovedDomainEvent = AssertPublishedDomainEvent<TransferRemovedDomainEvent>(transfer);

        Assert.That(transferRemovedDomainEvent.TransferId, Is.EqualTo(transfer.Id));
    }
}
