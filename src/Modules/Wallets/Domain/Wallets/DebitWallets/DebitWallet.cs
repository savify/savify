using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets.DebitWallets.Events;
using App.Modules.Wallets.Domain.Wallets.DebitWallets.Rules;

namespace App.Modules.Wallets.Domain.Wallets.DebitWallets;

public class DebitWallet : Entity, IAggregateRoot
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

    public static DebitWallet AddNew(UserId userId, string title, Currency currency, int balance = 0)
    {
        return new DebitWallet(userId, title, currency, balance);
    }

    public void Edit(string? newTitle, Currency? newCurrency, int? newBalance)
    {
        // TODO: restrict updating currency and balance for wallets that were connected to bank accounts
        CheckRules(new DebitWalletCannotBeEditedIfWasRemovedRule(Id, _isRemoved));

        _title = newTitle ?? _title;
        _currency = newCurrency ?? _currency;
        _balance = newBalance ?? _balance;
        _updatedAt = DateTime.UtcNow;

        AddDomainEvent(new DebitWalletEditedDomainEvent(Id, UserId, newCurrency, newBalance));
    }

    public void Remove()
    {
        // TODO: check if there is a need to set some rules on wallet removal
        CheckRules(new DebitWalletCannotBeRemovedMoreThanOnceRule(Id, _isRemoved));

        _isRemoved = true;
        _removedAt = DateTime.UtcNow;

        AddDomainEvent(new DebitWalletRemovedDomainEvent(Id, UserId));
    }

    public async Task<BankConnectionProcess> InitiateBankConnectionProcess(BankId bankId, IBankConnectionProcessInitiationService initiationService)
    {
        return await BankConnectionProcess.Initiate(UserId, bankId, Id, initiationService);
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
