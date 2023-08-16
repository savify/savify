using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets.DebitWallets.Events;

namespace App.Modules.Wallets.Domain.Wallets.DebitWallets;

public class DebitWallet : Entity, IAggregateRoot
{
    public WalletId Id { get; private set; }

    internal UserId UserId { get; private set; }

    private string _title;

    private Currency _currency;

    private int _balance;

    private DateTime _createdAt;

    public static DebitWallet AddNew(UserId userId, string title, Currency currency, int balance = 0)
    {
        return new DebitWallet(userId, title, currency, balance);
    }

    private DebitWallet(UserId userId, string title, Currency currency, int balance)
    {
        Id = new WalletId(Guid.NewGuid());
        UserId = userId;
        _title = title;
        _currency = currency;
        _balance = balance;
        _createdAt = DateTime.UtcNow;

        AddDomainEvent(new DebitWalletAddedDomainEvent(Id, userId, _currency));
    }

    private DebitWallet()
    { }
}
