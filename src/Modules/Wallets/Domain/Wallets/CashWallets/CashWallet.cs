using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets.CashWallets.Events;

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

    public static CashWallet AddNew(UserId userId, string title, Currency currency, int balance = 0)
    {
        return new CashWallet(userId, title, currency, balance);
    }

    public void Edit(string? newTitle, Currency? newCurrency, int? newBalance)
    {
        _title = newTitle ?? _title;
        _currency = newCurrency ?? _currency;
        _balance = newBalance ?? _balance;
        _updatedAt = DateTime.UtcNow;

        AddDomainEvent(new CashWalletEditedDomainEvent(Id, UserId, newCurrency, newBalance));
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
