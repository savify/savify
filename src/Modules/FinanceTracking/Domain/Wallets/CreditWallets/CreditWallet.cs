using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets.Rules;

namespace App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

public class CreditWallet : Entity, IAggregateRoot
{
    public WalletId Id { get; private set; }

    public UserId UserId { get; private set; }

    private string _title;

    private int _availableBalance;

    private int _creditLimit;

    private Currency _currency;

    private DateTime _createdAt;

    private DateTime? _updatedAt;

    private DateTime? _removedAt;

    private bool _isRemoved;

    public static CreditWallet AddNew(UserId userId, string title, Currency currency, int creditLimit, int availableBalance)
    {
        return new CreditWallet(userId, title, currency, creditLimit, availableBalance);
    }

    public void Edit(string? newTitle, int? newAvailableBalance, int? newCreditLimit)
    {
        // TODO: restrict currency, available balance and credit limit edtion for wallets that have bank account connected
        CheckRules(new CreditWalletCannotBeEditedIfWasRemovedRule(Id, _isRemoved));

        _title = newTitle ?? _title;
        _availableBalance = newAvailableBalance ?? _availableBalance;
        _creditLimit = newCreditLimit ?? _creditLimit;
        _updatedAt = DateTime.UtcNow;

        AddDomainEvent(new CreditWalletEditedDomainEvent(Id, UserId, newAvailableBalance, newCreditLimit));
    }

    public void Remove()
    {
        // TODO: check if there is a need to set some rules on wallet removal
        CheckRules(new CreditWalletCannotBeRemovedMoreThanOnceRule(Id, _isRemoved));

        _isRemoved = true;
        _removedAt = DateTime.UtcNow;

        AddDomainEvent(new CreditWalletRemovedDomainEvent(Id, UserId));
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
