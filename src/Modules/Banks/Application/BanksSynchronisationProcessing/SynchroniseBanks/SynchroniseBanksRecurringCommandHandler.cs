using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;

namespace App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;

internal class SynchroniseBanksRecurringCommandHandler(
    IBanksSynchronisationProcessRepository banksSynchronisationProcessRepository,
    IBanksSynchronisationService banksSynchronisationService)
    : ICommandHandler<SynchroniseBanksRecurringCommand>
{
    public async Task Handle(SynchroniseBanksRecurringCommand command, CancellationToken cancellationToken)
    {
        var banksSynchronisationProcess = await BanksSynchronisationProcess.Start(
            BanksSynchronisationProcessInitiator.InternalCommand,
            banksSynchronisationService);

        await banksSynchronisationProcessRepository.AddAsync(banksSynchronisationProcess);
    }
}
