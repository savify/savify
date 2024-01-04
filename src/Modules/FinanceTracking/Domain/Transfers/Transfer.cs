using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Transfers.Rules;
using App.Modules.FinanceTracking.Domain.Users;
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

    //TODO: Refactor to IEnumerable when Ngpsql adapter for EF will be fixed.
    //Now it throws an exception for IEnumerable primitive collection when
    //the entity is constructing by the EF (query operation on DbContext)
    //https://github.com/npgsql/efcore.pg/issues/3038
    private string[] _tags;

    public static Transfer AddNew(
        UserId userId,
        WalletId sourceWalletId,
        WalletId targetWalletId,
        Money amount,
        DateTime madeOn,
        IWalletsRepository walletsRepository,
        string? comment,
        IEnumerable<string>? tags)
    {
        return new Transfer(userId, sourceWalletId, targetWalletId, amount, madeOn, walletsRepository, comment, tags);
    }

    public void Edit(
        UserId userId,
        WalletId newSourceWalletId,
        WalletId newTargetWalletId,
        Money newAmount,
        DateTime newMadeOn,
        IWalletsRepository walletsRepository,
        string? newComment,
        IEnumerable<string>? newTags)
    {
        CheckRules(
            new TransferSourceAndTargetWalletsMustBeDifferentRule(newSourceWalletId, newTargetWalletId),
            new TransferSourceAndTargetMustBeOwnedByTheSameUserRule(userId, newSourceWalletId, newTargetWalletId, walletsRepository));

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
        _comment = newComment ?? string.Empty;
        _tags = newTags?.ToArray() ?? [];

        AddDomainEvent(new TransferEditedDomainEvent(
            userId,
            oldSourceWalletId,
            _sourceWalletId,
            oldTargetWalletId,
            _targetWalletId,
            oldAmount,
            _amount,
            _tags));
    }

    public void Remove()
    {
        AddDomainEvent(new TransferRemovedDomainEvent(Id));
    }

    private Transfer(
        UserId userId,
        WalletId sourceWalletId,
        WalletId targetWalletId,
        Money amount,
        DateTime madeOn,
        IWalletsRepository walletsRepository,
        string? comment,
        IEnumerable<string>? tags)
    {
        CheckRules(
            new TransferSourceAndTargetWalletsMustBeDifferentRule(sourceWalletId, targetWalletId),
            new TransferSourceAndTargetMustBeOwnedByTheSameUserRule(userId, sourceWalletId, targetWalletId, walletsRepository));

        Id = new TransferId(Guid.NewGuid());
        _sourceWalletId = sourceWalletId;
        _targetWalletId = targetWalletId;
        _amount = amount;
        _madeOn = madeOn;
        _comment = comment ?? string.Empty;
        _tags = tags?.ToArray() ?? [];

        AddDomainEvent(new TransferAddedDomainEvent(Id, userId, _sourceWalletId, _targetWalletId, _amount, _tags));
    }

    private Transfer()
    { }
}
