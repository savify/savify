using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets.CreditWallets.Events;

namespace App.Modules.Wallets.Domain.Wallets.CreditWallets;

public class CreditWallet : Entity, IAggregateRoot
{
    public WalletId Id { get; private set; }

    internal UserId UserId { get; private set; }

    private string _title;

    private int _availableBalance;

    private int _creditLimit;

    private Currency _currency;

    private DateTime _createdAt;

    public static CreditWallet AddNew(UserId userId, string title, Currency currency, int creditLimit, int availableBalance)
    {
        return new CreditWallet(userId, title, currency, creditLimit, availableBalance);
    }

    private CreditWallet(UserId userId, string title, Currency currency, int creditLimit, int availableBalance)
    {
        Id = new WalletId(Guid.NewGuid());
        UserId = userId;
        _title = title;
        _availableBalance = availableBalance;
        _creditLimit = creditLimit;
        _currency = currency;
        _createdAt = DateTime.UtcNow;

        AddDomainEvent(new CreditWalletAddedDomainEvent(Id, UserId, _currency));
    }

    private CreditWallet() { }
}
