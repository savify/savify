using App.BuildingBlocks.Domain;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Events;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing;

public class BanksSynchronisationProcess : Entity, IAggregateRoot
{
    public BanksSynchronisationProcessId Id { get; private set; }

    private BanksSynchronisationProcessStatus _status;

    private BanksSynchronisationProcessInitiator _initiatedBy;

    private DateTime _startedAt;

    private DateTime? _finishedAt;

    public BanksSynchronisationProcessStatus Status => _status;

    public static async Task<BanksSynchronisationProcess> Start(
        BanksSynchronisationProcessInitiator initiatedBy,
        IBanksSynchronisationService banksSynchronisationService)
    {
        var banksSynchronisationProcess = new BanksSynchronisationProcess(initiatedBy);

        var result = await banksSynchronisationService.SynchroniseAsync(banksSynchronisationProcess.Id);
        if (result.IsSuccess)
        {
            banksSynchronisationProcess.Finish();
        }
        else
        {
            banksSynchronisationProcess.Fail();
        }

        return banksSynchronisationProcess;
    }

    public void Finish()
    {
        _status = BanksSynchronisationProcessStatus.Finished;
        _finishedAt = DateTime.UtcNow;

        AddDomainEvent(new BanksSynchronisationProcessFinishedDomainEvent(Id));
    }

    public void Fail()
    {
        _status = BanksSynchronisationProcessStatus.Failed;
        _finishedAt = DateTime.UtcNow;

        AddDomainEvent(new BanksSynchronisationProcessFailedDomainEvent(Id));
    }

    public BanksSynchronisationProcess(BanksSynchronisationProcessInitiator initiatedBy)
    {
        Id = new BanksSynchronisationProcessId(Guid.NewGuid());
        _status = BanksSynchronisationProcessStatus.Started;
        _initiatedBy = initiatedBy;
        _startedAt = DateTime.UtcNow;

        AddDomainEvent(new BanksSynchronisationProcessStartedDomainEvent(Id, _initiatedBy));
    }

    private BanksSynchronisationProcess() { }
}
