using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;
using App.Modules.Banks.Domain.Users;

namespace App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;

internal class SynchroniseBanksCommandHandler : ICommandHandler<SynchroniseBanksCommand, BanksSynchronisationResultDto>
{
    private readonly IBanksSynchronisationProcessRepository _banksSynchronisationProcessRepository;

    private readonly ILastSuccessfulBanksSynchronisationProcessAccessor _lastSuccessfulBanksSynchronisationProcessAccessor;

    private readonly IBanksSynchronisationService _banksSynchronisationService;

    public SynchroniseBanksCommandHandler(
        IBanksSynchronisationProcessRepository banksSynchronisationProcessRepository,
        ILastSuccessfulBanksSynchronisationProcessAccessor lastSuccessfulBanksSynchronisationProcessAccessor,
        IBanksSynchronisationService banksSynchronisationService)
    {
        _banksSynchronisationProcessRepository = banksSynchronisationProcessRepository;
        _lastSuccessfulBanksSynchronisationProcessAccessor = lastSuccessfulBanksSynchronisationProcessAccessor;
        _banksSynchronisationService = banksSynchronisationService;
    }

    public async Task<BanksSynchronisationResultDto> Handle(SynchroniseBanksCommand command, CancellationToken cancellationToken)
    {
        var banksSynchronisationProcess = await BanksSynchronisationProcess.Start(
            BanksSynchronisationProcessInitiator.User(new UserId(command.UserId)),
            _banksSynchronisationService,
            _lastSuccessfulBanksSynchronisationProcessAccessor);

        await _banksSynchronisationProcessRepository.AddAsync(banksSynchronisationProcess);

        return new BanksSynchronisationResultDto(banksSynchronisationProcess.GetStatus().Value);
    }
}
