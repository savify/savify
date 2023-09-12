using App.BuildingBlocks.Domain;
using App.Modules.Banks.Domain.BankSynchronisationProcessing.Events;
using App.Modules.Banks.Domain.BankSynchronisationProcessing.Exceptions;
using App.Modules.Banks.Domain.BankSynchronisationProcessing.Services;

namespace App.Modules.Banks.Domain.BankSynchronisationProcessing;

public class BankSynchronisationProcess : Entity, IAggregateRoot
{
    public BankSynchronisationProcessId Id { get; private set; }

    private BankSynchronisationProcessStatus _status;

    private Initiator _initiatedBy;

    private DateTime _startedAt;

    private DateTime? _finishedAt;

    public static async Task<BankSynchronisationProcess> Start(Initiator initiatedBy, IBankSynchronisationService bankSynchronisationService)
    {
        var bankSynchronisationProcess = new BankSynchronisationProcess(initiatedBy);

        // TODO: for now synchronisation process will be synchronous; if execution time will be too large, we need to handle it asynchronous
        try
        {
            await bankSynchronisationService.Synchronise(bankSynchronisationProcess.Id);
            bankSynchronisationProcess.Finish();
        }
        catch (BankSynchronisationProcessException)
        {
            bankSynchronisationProcess.Fail();
        }

        return bankSynchronisationProcess;
    }

    public void Finish()
    {
        _status = BankSynchronisationProcessStatus.Finished;
        _finishedAt = DateTime.UtcNow;
    }

    public void Fail()
    {
        _status = BankSynchronisationProcessStatus.Failed;
        _finishedAt = DateTime.UtcNow;
    }

    public BankSynchronisationProcess(Initiator initiatedBy)
    {
        Id = new BankSynchronisationProcessId(Guid.NewGuid());
        _status = BankSynchronisationProcessStatus.Started;
        _initiatedBy = initiatedBy;
        _startedAt = DateTime.UtcNow;

        AddDomainEvent(new BankSynchronisationProcessStartedEvent(Id, _initiatedBy));
    }

    private BankSynchronisationProcess() { }
}
