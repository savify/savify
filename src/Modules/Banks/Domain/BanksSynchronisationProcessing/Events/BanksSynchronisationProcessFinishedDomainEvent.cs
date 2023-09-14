using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing.Events;

public class BanksSynchronisationProcessFinishedDomainEvent : DomainEventBase
{
    public BanksSynchronisationProcessId BanksSynchronisationProcessId { get; }


    public BanksSynchronisationProcessFinishedDomainEvent(BanksSynchronisationProcessId banksSynchronisationProcessId)
    {
        BanksSynchronisationProcessId = banksSynchronisationProcessId;
    }
}
