using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Finance;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets.CashWallets.Events;
using App.Modules.Wallets.Domain.Wallets.CashWallets.Rules;

namespace App.Modules.Wallets.Domain.Wallets.CashWallets;

public class CashWallet : Entity, IAggregateRoot
{
    public WalletId Id { get; private set; }

    public UserId UserId { get; private set; }

    private string _title;

    private Currency _currency;

    private int _balance;

    private DateTime _createdAt;

    private DateTime? _updatedAt = null;

    private DateTime? _removedAt = null;

    private bool _isRemoved = false;

    public static CashWallet AddNew(UserId userId, string title, Currency currency, int balance = 0)
    {
        return new CashWallet(userId, title, currency, balance);
    }

    public void Edit(string? newTitle, Currency? newCurrency, int? newBalance)
    {
        CheckRules(new CashWalletCannotBeEditedIfWasRemovedRule(Id, _isRemoved));

        _title = newTitle ?? _title;
        _currency = newCurrency ?? _currency;
        _balance = newBalance ?? _balance;
        _updatedAt = DateTime.UtcNow;

        AddDomainEvent(new CashWalletEditedDomainEvent(Id, UserId, newCurrency, newBalance));
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
