using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Transfers.Events;
using App.Modules.FinanceTracking.Domain.Transfers.Rules;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Transfers;

public class Transfer : Entity, IAggregateRoot
{
    public TransferId Id { get; private init; }

    public UserId UserId { get; private set; }

    private WalletId _sourceWalletId;

    private WalletId _targetWalletId;

    private TransferAmount _amount;

    private DateTime _madeOn;

    private string _comment;

    //TODO: Refactor to IEnumerable when Ngpsql adapter for EF will be fixed.
    //Now it throws an exception for IEnumerable primitive collection when
    //the entity is constructing by the EF (query operation on DbContext)
    //https://github.com/npgsql/efcore.pg/issues/3038
    private string[] _tags;

    public static Transfer AddNew(
        UserId userId,
        Wallet sourceWallet,
        Wallet targetWallet,
        TransferAmount amount,
        DateTime madeOn,
        string? comment,
        IEnumerable<string>? tags)
    {
        return new Transfer(userId, sourceWallet, targetWallet, amount, madeOn, comment, tags);
    }

    public void Edit(
        Wallet newSourceWallet,
        Wallet newTargetWallet,
        TransferAmount newAmount,
        DateTime newMadeOn,
        string? newComment,
        IEnumerable<string>? newTags)
    {
        CheckRules(
            new TransferSourceAndTargetWalletsMustBeDifferentRule(newSourceWallet.Id, newTargetWallet.Id),
            new TransferSourceAndTargetMustBeOwnedByTheSameUserRule(UserId, newSourceWallet, newTargetWallet));

        var oldSourceWalletId = _sourceWalletId;
        var oldTargetWalletId = _targetWalletId;
        var oldAmount = _amount;

        _sourceWalletId = newSourceWallet.Id;
        _targetWalletId = newTargetWallet.Id;
        _amount = newAmount;
        _madeOn = newMadeOn;
        _comment = newComment ?? string.Empty;
        _tags = newTags?.ToArray() ?? [];

        AddDomainEvent(new TransferEditedDomainEvent(
            UserId,
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
        AddDomainEvent(new TransferRemovedDomainEvent(Id, _sourceWalletId, _targetWalletId, _amount));
    }

    private Transfer(
        UserId userId,
        Wallet sourceWallet,
        Wallet targetWallet,
        TransferAmount amount,
        DateTime madeOn,
        string? comment,
        IEnumerable<string>? tags)
    {
        CheckRules(
            new TransferSourceAndTargetWalletsMustBeDifferentRule(sourceWallet.Id, targetWallet.Id),
            new TransferSourceAndTargetMustBeOwnedByTheSameUserRule(userId, sourceWallet, targetWallet));

        Id = new TransferId(Guid.NewGuid());
        UserId = userId;
        _sourceWalletId = sourceWallet.Id;
        _targetWalletId = targetWallet.Id;
        _amount = amount;
        _madeOn = madeOn;
        _comment = comment ?? string.Empty;
        _tags = tags?.ToArray() ?? [];

        AddDomainEvent(new TransferAddedDomainEvent(Id, UserId, _sourceWalletId, _targetWalletId, _amount, _tags));
    }

    private Transfer() { }
}
