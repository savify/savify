using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing.Events;

public class BanksSynchronisationProcessFailedDomainEvent : DomainEventBase
{
    public BanksSynchronisationProcessId BanksSynchronisationProcessId { get; }

    public BanksSynchronisationProcessFailedDomainEvent(BanksSynchronisationProcessId banksSynchronisationProcessId)
    {
        BanksSynchronisationProcessId = banksSynchronisationProcessId;
    }
}
