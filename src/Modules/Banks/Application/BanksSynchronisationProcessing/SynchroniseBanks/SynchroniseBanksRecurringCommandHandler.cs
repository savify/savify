using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;

namespace App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;

internal class SynchroniseBanksRecurringCommandHandler : ICommandHandler<SynchroniseBanksRecurringCommand>
{
    private readonly IBanksSynchronisationProcessRepository _banksSynchronisationProcessRepository;

    private readonly IBanksSynchronisationService _banksSynchronisationService;

    public SynchroniseBanksRecurringCommandHandler(
        IBanksSynchronisationProcessRepository banksSynchronisationProcessRepository,
        IBanksSynchronisationService banksSynchronisationService)
    {
        _banksSynchronisationProcessRepository = banksSynchronisationProcessRepository;
        _banksSynchronisationService = banksSynchronisationService;
    }

    public async Task Handle(SynchroniseBanksRecurringCommand command, CancellationToken cancellationToken)
    {
        var banksSynchronisationProcess = await BanksSynchronisationProcess.Start(
            BanksSynchronisationProcessInitiator.InternalCommand,
            _banksSynchronisationService);

        await _banksSynchronisationProcessRepository.AddAsync(banksSynchronisationProcess);
    }
}
