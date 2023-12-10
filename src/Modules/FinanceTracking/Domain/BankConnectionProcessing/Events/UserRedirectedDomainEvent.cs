using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Events;

public class UserRedirectedDomainEvent : DomainEventBase
{
    public BankConnectionProcessId BankConnectionProcessId { get; }

    public DateTime ExpiresAt { get; }

    public UserRedirectedDomainEvent(BankConnectionProcessId bankConnectionProcessId, DateTime expiresAt)
    {
        BankConnectionProcessId = bankConnectionProcessId;
        ExpiresAt = expiresAt;
    }
}
