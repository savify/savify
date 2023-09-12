using App.BuildingBlocks.Domain;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Events;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Exceptions;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing;

public class BanksSynchronisationProcess : Entity, IAggregateRoot
{
    public BanksSynchronisationProcessId Id { get; private set; }

    private BanksSynchronisationProcessStatus _status;

    private BanksSynchronisationProcessInitiator _initiatedBy;

    private DateTime _startedAt;

    private DateTime? _finishedAt;

    public static async Task<BanksSynchronisationProcess> Start(BanksSynchronisationProcessInitiator initiatedBy, IBanksSynchronisationService banksSynchronisationService)
    {
        var banksSynchronisationProcess = new BanksSynchronisationProcess(initiatedBy);

        // TODO: for now synchronisation process will be synchronous; if execution time will be too large, we need to handle it asynchronous
        try
        {
            await banksSynchronisationService.Synchronise(banksSynchronisationProcess.Id);
            banksSynchronisationProcess.Finish();
        }
        catch (BanksSynchronisationProcessException)
        {
            banksSynchronisationProcess.Fail();
        }

        return banksSynchronisationProcess;
    }

    public void Finish()
    {
        _status = BanksSynchronisationProcessStatus.Finished;
        _finishedAt = DateTime.UtcNow;
    }

    public void Fail()
    {
        _status = BanksSynchronisationProcessStatus.Failed;
        _finishedAt = DateTime.UtcNow;
    }

    public BanksSynchronisationProcess(BanksSynchronisationProcessInitiator initiatedBy)
    {
        Id = new BanksSynchronisationProcessId(Guid.NewGuid());
        _status = BanksSynchronisationProcessStatus.Started;
        _initiatedBy = initiatedBy;
        _startedAt = DateTime.UtcNow;

        AddDomainEvent(new BanksSynchronisationProcessStartedEvent(Id, _initiatedBy));
    }

    private BanksSynchronisationProcess() { }
}
