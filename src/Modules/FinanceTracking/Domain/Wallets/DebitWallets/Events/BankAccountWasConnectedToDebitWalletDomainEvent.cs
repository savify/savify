using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Events;

public class BankAccountWasConnectedToDebitWalletDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }

    public UserId UserId { get; }

    public BankConnectionId BankConnectionId { get; }

    public BankAccountId BankAccountId { get; }

    public BankAccountWasConnectedToDebitWalletDomainEvent(WalletId walletId, UserId userId, BankConnectionId bankConnectionId, BankAccountId bankAccountId)
    {
        WalletId = walletId;
        UserId = userId;
        BankConnectionId = bankConnectionId;
        BankAccountId = bankAccountId;
    }
}
