using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Transfers.Rules;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

namespace App.Modules.FinanceTracking.UnitTests.Transfers;

[TestFixture]
public class TransferTests : UnitTestBase
{
    [Test]
    public void AddingNewTransfer_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var targetWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));

        var amount = TransactionAmount.From(Money.From(100, Currency.From("PLN")));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var transfer = Transfer.AddNew(userId, sourceWallet, targetWallet, amount, madeOn, comment, tags);

        var transferAddedDomainEvent = AssertPublishedDomainEvent<TransferAddedDomainEvent>(transfer);

        Assert.That(transferAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(transferAddedDomainEvent.SourceWalletId, Is.EqualTo(sourceWallet.Id));
        Assert.That(transferAddedDomainEvent.TargetWalletId, Is.EqualTo(targetWallet.Id));
        Assert.That(transferAddedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(transferAddedDomainEvent.Tags, Is.EquivalentTo(tags));
    }

    [Test]
    public void AddingNewTransfer_WhenSourceAndTargetWalletIdsAreTheSame_BreaksTransferSourceAndTargetWalletsMustBeDifferentRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var targetWallet = sourceWallet;

        var amount = TransactionAmount.From(Money.From(100, Currency.From("PLN")));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        AssertBrokenRule<TransferSourceAndTargetWalletsMustBeDifferentRule>(() =>
        {
            _ = Transfer.AddNew(userId, sourceWallet, targetWallet, amount, madeOn, comment, tags);
        });
    }

    [Test]
    public void AddingNewTransfer_WhenUserDoesNotOwnOneOfWallets_BreaksTransferSourceAndTargetMustBeOwnedByTheSameUserRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var targetWallet = CashWallet.AddNew(new UserId(Guid.NewGuid()), "Cash", Currency.From("PLN"));

        var amount = TransactionAmount.From(Money.From(100, Currency.From("PLN")));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        AssertBrokenRule<TransferSourceAndTargetMustBeOwnedByTheSameUserRule>(() =>
        {
            _ = Transfer.AddNew(userId, sourceWallet, targetWallet, amount, madeOn, comment, tags);
        });
    }

    [Test]
    public void EditingTransfer_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var targetWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));

        var amount = TransactionAmount.From(Money.From(100, Currency.From("PLN")));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var transfer = Transfer.AddNew(userId, sourceWallet, targetWallet, amount, madeOn, comment, tags);

        var newSourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var newTargetWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));

        var newAmount = TransactionAmount.From(Money.From(500, Currency.From("PLN")));
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        transfer.Edit(newSourceWallet, newTargetWallet, newAmount, newMadeOn, newComment, newTags);

        var transferEditedDomainEvent = AssertPublishedDomainEvent<TransferEditedDomainEvent>(transfer);

        Assert.That(transferEditedDomainEvent.OldSourceWalletId, Is.EqualTo(sourceWallet.Id));
        Assert.That(transferEditedDomainEvent.NewSourceWalletId, Is.EqualTo(newSourceWallet.Id));

        Assert.That(transferEditedDomainEvent.OldTargetWalletId, Is.EqualTo(targetWallet.Id));
        Assert.That(transferEditedDomainEvent.NewTargetWalletId, Is.EqualTo(newTargetWallet.Id));

        Assert.That(transferEditedDomainEvent.OldAmount.Source, Is.EqualTo(amount.Source));
        Assert.That(transferEditedDomainEvent.NewAmount.Source, Is.EqualTo(newAmount.Source));

        Assert.That(transferEditedDomainEvent.OldAmount.Target, Is.EqualTo(amount.Target));
        Assert.That(transferEditedDomainEvent.NewAmount.Target, Is.EqualTo(newAmount.Target));

        Assert.That(transferEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(transferEditedDomainEvent.Tags, Is.EqualTo(newTags));
    }

    [Test]
    public void EditingTransfer_WhenNewSourceAndTargetWalletIdsAreTheSame_BreaksTransferSourceAndTargetWalletsMustBeDifferentRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var targetWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));

        var amount = TransactionAmount.From(Money.From(100, Currency.From("PLN")));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var transfer = Transfer.AddNew(userId, sourceWallet, targetWallet, amount, madeOn, comment, tags);

        var newSourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var newTargetWallet = newSourceWallet;

        var newAmount = TransactionAmount.From(Money.From(500, Currency.From("PLN")));
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        AssertBrokenRule<TransferSourceAndTargetWalletsMustBeDifferentRule>(() =>
        {
            transfer.Edit(newSourceWallet, newTargetWallet, newAmount, newMadeOn, newComment, newTags);
        });
    }

    [Test]
    public void EditingTransfer_WhenUserDoesNotOwnOneOfNewWallets_BreaksTransferSourceAndTargetMustBeOwnedByTheSameUserRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var targetWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));

        var amount = TransactionAmount.From(Money.From(100, Currency.From("PLN")));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var transfer = Transfer.AddNew(userId, sourceWallet, targetWallet, amount, madeOn, comment, tags);

        var newSourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var newTargetWallet = CashWallet.AddNew(new UserId(Guid.NewGuid()), "Cash", Currency.From("PLN"));

        var newAmount = TransactionAmount.From(Money.From(500, Currency.From("PLN")));
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        AssertBrokenRule<TransferSourceAndTargetMustBeOwnedByTheSameUserRule>(() =>
        {
            transfer.Edit(newSourceWallet, newTargetWallet, newAmount, newMadeOn, newComment, newTags);
        });
    }

    [Test]
    public void RemovingTransfer_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var targetWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));

        var amount = TransactionAmount.From(Money.From(100, Currency.From("PLN")));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var transfer = Transfer.AddNew(userId, sourceWallet, targetWallet, amount, madeOn, comment, tags);

        transfer.Remove();

        var transferRemovedDomainEvent = AssertPublishedDomainEvent<TransferRemovedDomainEvent>(transfer);

        Assert.That(transferRemovedDomainEvent.TransferId, Is.EqualTo(transfer.Id));
        Assert.That(transferRemovedDomainEvent.SourceWalletId, Is.EqualTo(sourceWallet.Id));
        Assert.That(transferRemovedDomainEvent.TargetWalletId, Is.EqualTo(targetWallet.Id));
        Assert.That(transferRemovedDomainEvent.Amount, Is.EqualTo(amount));
    }
}
