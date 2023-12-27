using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Transfers.Rules;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers;

public class Transfer : Entity, IAggregateRoot
{
    public TransferId Id { get; private init; }

    private WalletId _sourceWalletId;

    private WalletId _targetWalletId;

    private Money _amount;

    private CategoryId _categoryId;

    private DateTime _madeOn;

    private string _comment;

    //TODO: Refactor to IEnumerable when Ngpsql adapter for EF will be fixed.
    //Now it throws an exception for IEnumerable primitive collection when
    //the entity is constructing by the EF (query operation on DbContext)
    private string[] _tags;

    public static Transfer AddNew(WalletId sourceWalletId, WalletId targetWalletId, Money amount, CategoryId categoryId, DateTime madeOn, string comment, IEnumerable<string> tags)
    {
        return new Transfer(sourceWalletId, targetWalletId, amount, categoryId, madeOn, comment, tags);
    }

    public void Edit(WalletId newSourceWalletId, WalletId newTargetWalletId, Money newAmount, CategoryId newCategoryId, DateTime newMadeOn, string newComment, IEnumerable<string> newTags)
    {
        CheckRules(new TransferAmountMustBeBiggerThanZeroRule(newAmount),
                   new TransferSourceAndTargetWalletsMustBeDifferentRule(newSourceWalletId, newTargetWalletId));

        var oldSourceWalletId = _sourceWalletId;
        var oldTargetWalletId = _targetWalletId;
        var oldAmount = _amount;
        var oldMadeOn = _madeOn;
        var oldCategoryId = _categoryId;
        var oldComment = _comment;
        var oldTags = _tags;

        _sourceWalletId = newSourceWalletId;
        _targetWalletId = newTargetWalletId;
        _amount = newAmount;
        _categoryId = newCategoryId;
        _madeOn = newMadeOn;
        _comment = newComment;
        _tags = newTags.ToArray();

        AddDomainEvent(new TransferEditedDomainEvent(
            oldSourceWalletId,
            _sourceWalletId,
            oldTargetWalletId,
            _targetWalletId,
            oldAmount,
            _amount,
            oldCategoryId,
            _categoryId,
            oldMadeOn,
            _madeOn,
            oldComment,
            _comment,
            oldTags,
            _tags));
    }

    public void Remove()
    {
        AddDomainEvent(new TransferRemovedDomainEvent(Id));
    }

    private Transfer(WalletId sourceWalletId, WalletId targetWalletId, Money amount, CategoryId categoryId, DateTime madeOn, string comment, IEnumerable<string> tags)
    {
        CheckRules(new TransferAmountMustBeBiggerThanZeroRule(amount),
                   new TransferSourceAndTargetWalletsMustBeDifferentRule(sourceWalletId, targetWalletId));

        Id = new TransferId(Guid.NewGuid());
        _sourceWalletId = sourceWalletId;
        _targetWalletId = targetWalletId;
        _amount = amount;
        _madeOn = madeOn;
        _categoryId = categoryId;
        _comment = comment;
        _tags = tags.ToArray();

        AddDomainEvent(new TransferAddedDomainEvent(Id, _sourceWalletId, _targetWalletId, _amount, _categoryId, _madeOn, _comment, _tags));
    }

    private Transfer()
    { }
}
