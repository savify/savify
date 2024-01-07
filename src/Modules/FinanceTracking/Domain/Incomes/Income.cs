using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Incomes.Rules;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Incomes;

public class Income : Entity, IAggregateRoot
{
    public IncomeId Id { get; private set; }

    public UserId UserId { get; private set; }

    private WalletId _targetWalletId;

    private CategoryId _categoryId;

    private Money _amount;

    private DateTime _madeOn;

    private string _comment;

    //TODO: Refactor to IEnumerable when Ngpsql adapter for EF will be fixed.
    //Now it throws an exception for IEnumerable primitive collection when
    //the entity is constructing by the EF (query operation on DbContext)
    //https://github.com/npgsql/efcore.pg/issues/3038
    private string[] _tags;

    public static Income AddNew(
        UserId userId,
        WalletId targetWalletId,
        CategoryId categoryId,
        Money amount,
        DateTime madeOn,
        IWalletsRepository walletsRepository,
        string? comment,
        IEnumerable<string>? tags)
    {
        return new Income(userId, targetWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);
    }

    public void Edit(
        WalletId newTargetWalletId,
        CategoryId newCategoryId,
        Money newAmount,
        DateTime newMadeOn,
        IWalletsRepository walletsRepository,
        string? newComment,
        IEnumerable<string>? newTags)
    {
        CheckRules(new IncomeTargetWalletMustBeOwnedByUserRule(UserId, newTargetWalletId, walletsRepository));

        var oldTargetWalletId = _targetWalletId;
        var oldAmount = _amount;

        _targetWalletId = newTargetWalletId;
        _categoryId = newCategoryId;
        _amount = newAmount;
        _madeOn = newMadeOn;
        _comment = newComment ?? string.Empty;
        _tags = newTags?.ToArray() ?? [];

        AddDomainEvent(new IncomeEditedDomainEvent(
            UserId,
            oldTargetWalletId,
            _targetWalletId,
            oldAmount,
            _amount,
            _tags));
    }

    public void Remove()
    {
        AddDomainEvent(new IncomeRemovedDomainEvent(Id, _targetWalletId, _amount));
    }

    private Income(
        UserId userId,
        WalletId targetWalletId,
        CategoryId categoryId,
        Money amount,
        DateTime madeOn,
        IWalletsRepository walletsRepository,
        string? comment,
        IEnumerable<string>? tags)
    {
        CheckRules(new IncomeTargetWalletMustBeOwnedByUserRule(userId, targetWalletId, walletsRepository));

        Id = new IncomeId(Guid.NewGuid());
        UserId = userId;
        _targetWalletId = targetWalletId;
        _categoryId = categoryId;
        _amount = amount;
        _madeOn = madeOn;
        _comment = comment ?? string.Empty;
        _tags = tags?.ToArray() ?? [];

        AddDomainEvent(new IncomeAddedDomainEvent(Id, UserId, _targetWalletId, _categoryId, _amount, _tags));
    }

    private Income() { }
}
