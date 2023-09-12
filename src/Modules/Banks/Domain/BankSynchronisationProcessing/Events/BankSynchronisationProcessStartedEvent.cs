using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.BankSynchronisationProcessing.Events;

public class BankSynchronisationProcessStartedEvent : DomainEventBase
{
    public BankSynchronisationProcessId BankSynchronisationProcessId { get; }

    public Initiator InitiatedBy { get; }

    public BankSynchronisationProcessStartedEvent(BankSynchronisationProcessId bankSynchronisationProcessId, Initiator initiatedBy)
    {
        BankSynchronisationProcessId = bankSynchronisationProcessId;
        InitiatedBy = initiatedBy;
    }
}
