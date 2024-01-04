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
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);
        walletsRepository.ExistsForUserIdAndWalletId(userId, targetWalletId).Returns(true);

        var transfer = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, walletsRepository, comment, tags);

        var transferAddedDomainEvent = AssertPublishedDomainEvent<TransferAddedDomainEvent>(transfer);

        Assert.That(transferAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(transferAddedDomainEvent.SourceWalletId, Is.EqualTo(sourceWalletId));
        Assert.That(transferAddedDomainEvent.TargetWalletId, Is.EqualTo(targetWalletId));
        Assert.That(transferAddedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(transferAddedDomainEvent.Tags, Is.EquivalentTo(tags));
    }

    [Test]
    public void AddingNewTransfer_WhenSourceAndTargetWalletIdsAreTheSame_BreaksTransferSourceAndTargetWalletsMustBeDifferentRule()
    {
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = sourceWalletId;

        var userId = new UserId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);
        walletsRepository.ExistsForUserIdAndWalletId(userId, targetWalletId).Returns(true);

        AssertBrokenRule<TransferSourceAndTargetWalletsMustBeDifferentRule>(() =>
        {
            _ = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, walletsRepository, comment, tags);
        });
    }

    [Test]
    public void AddingNewTransfer_WhenUserDoesNotOwnOneOfWallets_BreaksTransferSourceAndTargetMustBeOwnedByTheSameUserRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = new WalletId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);
        walletsRepository.ExistsForUserIdAndWalletId(userId, targetWalletId).Returns(false);

        AssertBrokenRule<TransferSourceAndTargetMustBeOwnedByTheSameUserRule>(() =>
        {
            _ = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, walletsRepository, comment, tags);
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
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);
        walletsRepository.ExistsForUserIdAndWalletId(userId, targetWalletId).Returns(true);

        var transfer = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, walletsRepository, comment, tags);

        var newSourceWalletId = new WalletId(Guid.NewGuid());
        var newTargetWalletId = new WalletId(Guid.NewGuid());
        var newAmount = Money.From(500, "USD");
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];
        walletsRepository.ExistsForUserIdAndWalletId(userId, newSourceWalletId).Returns(true);
        walletsRepository.ExistsForUserIdAndWalletId(userId, newTargetWalletId).Returns(true);

        transfer.Edit(userId, newSourceWalletId, newTargetWalletId, newAmount, newMadeOn, walletsRepository, newComment, newTags);

        var transferEditedDomainEvent = AssertPublishedDomainEvent<TransferEditedDomainEvent>(transfer);

        Assert.That(transferEditedDomainEvent.OldSourceWalletId, Is.EqualTo(sourceWalletId));
        Assert.That(transferEditedDomainEvent.NewSourceWalletId, Is.EqualTo(newSourceWalletId));

        Assert.That(transferEditedDomainEvent.OldTargetWalletId, Is.EqualTo(targetWalletId));
        Assert.That(transferEditedDomainEvent.NewTargetWalletId, Is.EqualTo(newTargetWalletId));

        Assert.That(transferEditedDomainEvent.OldAmount, Is.EqualTo(amount));
        Assert.That(transferEditedDomainEvent.NewAmount, Is.EqualTo(newAmount));

        Assert.That(transferEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(transferEditedDomainEvent.Tags, Is.EqualTo(newTags));
    }

    [Test]
    public void EditingTransfer_WhenNewSourceAndTargetWalletIdsAreTheSame_BreaksTransferSourceAndTargetWalletsMustBeDifferentRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = new WalletId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);
        walletsRepository.ExistsForUserIdAndWalletId(userId, targetWalletId).Returns(true);

        var transfer = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, walletsRepository, comment, tags);

        var newSourceWalletId = new WalletId(Guid.NewGuid());
        var newTargetWalletId = newSourceWalletId;
        var newAmount = Money.From(500, "USD");
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        AssertBrokenRule<TransferSourceAndTargetWalletsMustBeDifferentRule>(() =>
        {
            transfer.Edit(userId, newSourceWalletId, newTargetWalletId, newAmount, newMadeOn, walletsRepository, newComment, newTags);
        });
    }

    [Test]
    public void EditingTransfer_WhenUserDoesNotOwnOneOfNewWallets_BreaksTransferSourceAndTargetMustBeOwnedByTheSameUserRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var targetWalletId = new WalletId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);
        walletsRepository.ExistsForUserIdAndWalletId(userId, targetWalletId).Returns(true);

        var transfer = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, walletsRepository, comment, tags);

        var newSourceWalletId = new WalletId(Guid.NewGuid());
        var newTargetWalletId = new WalletId(Guid.NewGuid());
        var newAmount = Money.From(500, "USD");
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];
        walletsRepository.ExistsForUserIdAndWalletId(userId, newSourceWalletId).Returns(true);
        walletsRepository.ExistsForUserIdAndWalletId(userId, newTargetWalletId).Returns(false);

        AssertBrokenRule<TransferSourceAndTargetMustBeOwnedByTheSameUserRule>(() =>
        {
            transfer.Edit(userId, newSourceWalletId, newTargetWalletId, newAmount, newMadeOn, walletsRepository, newComment, newTags);
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
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);
        walletsRepository.ExistsForUserIdAndWalletId(userId, targetWalletId).Returns(true);

        var transfer = Transfer.AddNew(userId, sourceWalletId, targetWalletId, amount, madeOn, walletsRepository, comment, tags);

        transfer.Remove();

        var transferRemovedDomainEvent = AssertPublishedDomainEvent<TransferRemovedDomainEvent>(transfer);

        Assert.That(transferRemovedDomainEvent.TransferId, Is.EqualTo(transfer.Id));
    }
}
