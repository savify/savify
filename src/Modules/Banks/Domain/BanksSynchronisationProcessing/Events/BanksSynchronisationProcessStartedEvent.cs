using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing.Events;

public class BanksSynchronisationProcessStartedEvent : DomainEventBase
{
    public BanksSynchronisationProcessId BanksSynchronisationProcessId { get; }

    public BanksSynchronisationProcessInitiator InitiatedBy { get; }

    public BanksSynchronisationProcessStartedEvent(BanksSynchronisationProcessId banksSynchronisationProcessId, BanksSynchronisationProcessInitiator initiatedBy)
    {
        BanksSynchronisationProcessId = banksSynchronisationProcessId;
        InitiatedBy = initiatedBy;
    }
}
