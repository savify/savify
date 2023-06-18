using App.BuildingBlocks.Domain;
using App.Modules.Accounts.Domain.Accounts.CreditAccounts.Events;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Domain.Accounts.CreditAccounts;

public class CreditAccount : Entity, IAggregateRoot
{
    public AccountId Id { get; private set; }

    internal UserId UserId { get; private set; }

    private string _title;

    private int _availableBalance;

    private int _creditLimit;

    private Currency _currency;

    private DateTime _createdAt;

    public static CreditAccount AddNew(UserId userId, string title, Currency currency, int creditLimit, int availableBalance)
    {
        return new CreditAccount(userId, title, currency, creditLimit, availableBalance);
    }

    private CreditAccount(UserId userId, string title, Currency currency, int creditLimit, int availableBalance)
    {
        Id = new AccountId(Guid.NewGuid());
        UserId = userId;
        _title = title;
        _availableBalance = availableBalance;
        _creditLimit = creditLimit;
        _currency = currency;
        _createdAt = DateTime.UtcNow;

        AddDomainEvent(new CreditAccountAddedDomainEvent(Id, UserId, _currency));
    }

    private CreditAccount()
    { }
}
