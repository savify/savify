using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Expenses.Events;
using App.Modules.FinanceTracking.Domain.Expenses.Rules;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Expenses;

public class Expense : Entity, IAggregateRoot
{
    public ExpenseId Id { get; private set; }

    public UserId UserId { get; private set; }

    private WalletId _sourceWalletId;

    private CategoryId _categoryId;

    private Money _amount;

    private DateTime _madeOn;

    private string _comment;

    //TODO: Refactor to IEnumerable when Ngpsql adapter for EF will be fixed.
    //Now it throws an exception for IEnumerable primitive collection when
    //the entity is constructing by the EF (query operation on DbContext)
    //https://github.com/npgsql/efcore.pg/issues/3038
    private string[] _tags;

    public static Expense AddNew(
        UserId userId,
        Wallet sourceWallet,
        CategoryId categoryId,
        Money amount,
        DateTime madeOn,
        string? comment,
        IEnumerable<string>? tags)
    {
        return new Expense(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);
    }

    public void Edit(
        Wallet newSourceWallet,
        CategoryId newCategoryId,
        Money newAmount,
        DateTime newMadeOn,
        string? newComment,
        IEnumerable<string>? newTags)
    {
        CheckRules(new ExpenseSourceWalletMustBeOwnedByUserRule(UserId, newSourceWallet));

        var oldSourceWalletId = _sourceWalletId;
        var oldAmount = _amount;

        _sourceWalletId = newSourceWallet.Id;
        _categoryId = newCategoryId;
        _amount = newAmount;
        _madeOn = newMadeOn;
        _comment = newComment ?? string.Empty;
        _tags = newTags?.ToArray() ?? [];

        AddDomainEvent(new ExpenseEditedDomainEvent(
            UserId,
            oldSourceWalletId,
            _sourceWalletId,
            oldAmount,
            _amount,
            _tags));
    }

    public void Remove()
    {
        AddDomainEvent(new ExpenseRemovedDomainEvent(Id, _sourceWalletId, _amount));
    }

    private Expense(
        UserId userId,
        Wallet sourceWallet,
        CategoryId categoryId,
        Money amount,
        DateTime madeOn,
        string? comment,
        IEnumerable<string>? tags)
    {
        CheckRules(new ExpenseSourceWalletMustBeOwnedByUserRule(userId, sourceWallet));

        Id = new ExpenseId(Guid.NewGuid());
        UserId = userId;
        _sourceWalletId = sourceWallet.Id;
        _categoryId = categoryId;
        _amount = amount;
        _madeOn = madeOn;
        _comment = comment ?? string.Empty;
        _tags = tags?.ToArray() ?? [];

        AddDomainEvent(new ExpenseAddedDomainEvent(Id, UserId, _sourceWalletId, _categoryId, _amount, _tags));
    }

    private Expense() { }
}
