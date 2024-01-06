using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Rules;

namespace App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

public class CashWallet : Entity, IAggregateRoot
{
    public WalletId Id { get; private set; }

    public UserId UserId { get; private set; }

    private string _title;

    private Currency _currency;

    private int _balance;

    private DateTime _createdAt;

    private DateTime? _updatedAt;

    private DateTime? _removedAt;

    private bool _isRemoved;

    public static CashWallet AddNew(UserId userId, string title, Currency currency, int balance = 0)
    {
        return new CashWallet(userId, title, currency, balance);
    }

    public void Edit(string? newTitle, int? newBalance)
    {
        CheckRules(new CashWalletCannotBeEditedIfWasRemovedRule(Id, _isRemoved));

        _title = newTitle ?? _title;
        _balance = newBalance ?? _balance;
        _updatedAt = DateTime.UtcNow;

        AddDomainEvent(new CashWalletEditedDomainEvent(Id, UserId, newBalance));
    }

    public void Remove()
    {
        CheckRules(new CashWalletCannotBeRemovedMoreThanOnceRule(Id, _isRemoved));

        _isRemoved = true;
        _removedAt = DateTime.UtcNow;

        AddDomainEvent(new CashWalletRemovedDomainEvent(Id, UserId));
    }

    private CashWallet(UserId userId, string title, Currency currency, int balance)
    {
        Id = new WalletId(Guid.NewGuid());
        UserId = userId;
        _title = title;
        _currency = currency;
        _balance = balance;
        _createdAt = DateTime.UtcNow;

        AddDomainEvent(new CashWalletAddedDomainEvent(Id, UserId, _currency));
    }

    private CashWallet() { }
}
