using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;
using App.Modules.Banks.Domain.Users;

namespace App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;

internal class SynchroniseBanksCommandHandler(
    IBanksSynchronisationProcessRepository banksSynchronisationProcessRepository,
    IBanksSynchronisationService banksSynchronisationService)
    : ICommandHandler<SynchroniseBanksCommand, BanksSynchronisationResultDto>
{
    public async Task<BanksSynchronisationResultDto> Handle(SynchroniseBanksCommand command, CancellationToken cancellationToken)
    {
        var banksSynchronisationProcess = await BanksSynchronisationProcess.Start(
            BanksSynchronisationProcessInitiator.User(new UserId(command.UserId)),
            banksSynchronisationService);

        await banksSynchronisationProcessRepository.AddAsync(banksSynchronisationProcess);

        return new BanksSynchronisationResultDto(banksSynchronisationProcess.GetStatus().Value);
    }
}
