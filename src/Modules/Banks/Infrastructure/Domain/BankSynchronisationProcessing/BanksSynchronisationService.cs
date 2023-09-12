using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;
using App.Modules.Banks.Infrastructure.Integrations.SaltEdge;

namespace App.Modules.Banks.Infrastructure.Domain.BankSynchronisationProcessing;

public class BanksSynchronisationService : IBanksSynchronisationService
{
    private readonly ISaltEdgeIntegrationService _saltEdgeIntegrationService;

    private readonly IBankRepository _bankRepository;

    public BanksSynchronisationService(ISaltEdgeIntegrationService saltEdgeIntegrationService, IBankRepository bankRepository)
    {
        _saltEdgeIntegrationService = saltEdgeIntegrationService;
        _bankRepository = bankRepository;
    }

    public async Task Synchronise(BanksSynchronisationProcessId banksSynchronisationProcessId)
    {

    }
}
