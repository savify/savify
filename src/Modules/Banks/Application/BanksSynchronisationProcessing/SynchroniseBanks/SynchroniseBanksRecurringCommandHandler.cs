using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Contracts;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;

namespace App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;

public class SynchroniseBanksRecurringCommandHandler : ICommandHandler<SynchroniseBanksRecurringCommand, Result>
{
    private readonly IBanksSynchronisationProcessRepository _banksSynchronisationProcessRepository;

    private readonly ILastSuccessfulBanksSynchronisationProcessAccessor _lastSuccessfulBanksSynchronisationProcessAccessor;

    private readonly IBanksSynchronisationService _banksSynchronisationService;

    public SynchroniseBanksRecurringCommandHandler(
        IBanksSynchronisationProcessRepository banksSynchronisationProcessRepository,
        ILastSuccessfulBanksSynchronisationProcessAccessor lastSuccessfulBanksSynchronisationProcessAccessor,
        IBanksSynchronisationService banksSynchronisationService)
    {
        _banksSynchronisationProcessRepository = banksSynchronisationProcessRepository;
        _lastSuccessfulBanksSynchronisationProcessAccessor = lastSuccessfulBanksSynchronisationProcessAccessor;
        _banksSynchronisationService = banksSynchronisationService;
    }

    public async Task<Result> Handle(SynchroniseBanksRecurringCommand command, CancellationToken cancellationToken)
    {
        var banksSynchronisationProcess = await BanksSynchronisationProcess.Start(
            BanksSynchronisationProcessInitiator.InternalCommand,
            _banksSynchronisationService,
            _lastSuccessfulBanksSynchronisationProcessAccessor);

        await _banksSynchronisationProcessRepository.AddAsync(banksSynchronisationProcess);

        return Result.Success;
    }
}
