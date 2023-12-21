using App.BuildingBlocks.Domain;
using App.Modules.Categories.Domain.CategoriesSynchronisationProcessing.Events;

namespace App.Modules.Categories.Domain.CategoriesSynchronisationProcessing;

public class CategoriesSynchronisationProcess : Entity, IAggregateRoot
{
    public CategoriesSynchronisationProcessId Id { get; private set; }

    private CategoriesSynchronisationProcessStatus _status;

    private DateTime _startedAt;

    private DateTime? _finishedAt;

    public CategoriesSynchronisationProcessStatus Status => _status;

    public static async Task<CategoriesSynchronisationProcess> Start(ICategoriesSynchronisationService categoriesSynchronisationService)
    {
        var categoriesSynchronisationProcess = new CategoriesSynchronisationProcess();

        try
        {
            await categoriesSynchronisationService.SynchroniseAsync();

            categoriesSynchronisationProcess.Finish();
        }
        catch (CategoriesSynchronisationProcessException)
        {
            categoriesSynchronisationProcess.Fail();
        }

        return categoriesSynchronisationProcess;
    }

    public void Finish()
    {
        _status = CategoriesSynchronisationProcessStatus.Finished;
        _finishedAt = DateTime.UtcNow;

        AddDomainEvent(new CategoriesSynchronisationProcessFinishedDomainEvent(Id));
    }

    public void Fail()
    {
        _status = CategoriesSynchronisationProcessStatus.Failed;
        _finishedAt = DateTime.UtcNow;

        AddDomainEvent(new CategoriesSynchronisationProcessFailedDomainEvent(Id));
    }

    private CategoriesSynchronisationProcess()
    {
        Id = new CategoriesSynchronisationProcessId(Guid.NewGuid());
        _status = CategoriesSynchronisationProcessStatus.Started;
        _startedAt = DateTime.UtcNow;

        AddDomainEvent(new CategoriesSynchronisationProcessStartedDomainEvent(Id));
    }
}
