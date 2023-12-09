using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Events;

public class BankConnectionProcessInitiatedDomainEvent : DomainEventBase
{
    public BankConnectionProcessId BankConnectionProcessId { get; }

    public UserId UserId { get; }

    public BankId BankId { get; }

    public WalletId WalletId { get; }

    public BankConnectionProcessInitiatedDomainEvent(BankConnectionProcessId bankConnectionProcessId, UserId userId, BankId bankId, WalletId walletId)
    {
        BankConnectionProcessId = bankConnectionProcessId;
        UserId = userId;
        BankId = bankId;
        WalletId = walletId;
    }
}
