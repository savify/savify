using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Wallets.CashWallets.Events;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.CashWallets;

public class CashWallet : Entity, IAggregateRoot
{
    public WalletId Id { get; private set; }

    internal UserId UserId { get; private set; }

    private string _title;

    private Currency _currency;
    
    private int _balance;

    private DateTime _createdAt;

    public static CashWallet AddNew(UserId userId, string title, Currency currency, int balance = 0)
    {
        return new CashWallet(userId, title, currency, balance);
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

    private CashWallet() {}
}
