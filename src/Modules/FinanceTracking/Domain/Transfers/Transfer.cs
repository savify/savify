using App.BuildingBlocks.Domain;
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

    private DateTime _madeOn;

    private string _comment;

    private IEnumerable<string> _tags;

    public static Transfer AddNew(WalletId sourceWalletId, WalletId targetWalletId, Money amount, DateTime madeOn, string comment, IEnumerable<string> tags)
    {
        return new Transfer(sourceWalletId, targetWalletId, amount, madeOn, comment, tags);
    }

    public void Edit(WalletId newSourceWalletId, WalletId newTargetWalletId, Money newAmount, DateTime newMadeOn, string newComment, IEnumerable<string> newTags)
    {
        CheckRules(new TransferAmountMustBeBiggerThanZero(newAmount),
                   new TransferSourceAndTargetWalletsMustBeDifferentRule(newSourceWalletId, newTargetWalletId));

        var oldSourceWalletId = _sourceWalletId;
        var oldTargetWalletId = _targetWalletId;
        var oldAmount = _amount;
        var oldMadeOn = _madeOn;
        var oldComment = _comment;
        var oldTags = _tags;

        _sourceWalletId = newSourceWalletId;
        _targetWalletId = newTargetWalletId;
        _amount = newAmount;
        _madeOn = newMadeOn;
        _comment = newComment;
        _tags = newTags;

        AddDomainEvent(new TransferEditedDomainEvent(
            oldSourceWalletId,
            _sourceWalletId,
            oldTargetWalletId,
            _targetWalletId,
            oldAmount,
            _amount,
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

    private Transfer(WalletId sourceWalletId, WalletId targetWalletId, Money amount, DateTime madeOn, string comment, IEnumerable<string> tags)
    {
        CheckRules(new TransferAmountMustBeBiggerThanZero(amount),
                   new TransferSourceAndTargetWalletsMustBeDifferentRule(sourceWalletId, targetWalletId));

        Id = new TransferId(Guid.NewGuid());
        _sourceWalletId = sourceWalletId;
        _targetWalletId = targetWalletId;
        _amount = amount;
        _madeOn = madeOn;
        _comment = comment;
        _tags = tags;

        AddDomainEvent(new TransferAddedDomainEvent(Id, _sourceWalletId, _targetWalletId, _amount, _madeOn, _comment, _tags));
    }

    private Transfer()
    { }
}
