using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing.Events;

public class BanksSynchronisationProcessStartedDomainEvent : DomainEventBase
{
    public BanksSynchronisationProcessId BanksSynchronisationProcessId { get; }

    public BanksSynchronisationProcessInitiator InitiatedBy { get; }

    public BanksSynchronisationProcessStartedDomainEvent(BanksSynchronisationProcessId banksSynchronisationProcessId, BanksSynchronisationProcessInitiator initiatedBy)
    {
        BanksSynchronisationProcessId = banksSynchronisationProcessId;
        InitiatedBy = initiatedBy;
    }
}
