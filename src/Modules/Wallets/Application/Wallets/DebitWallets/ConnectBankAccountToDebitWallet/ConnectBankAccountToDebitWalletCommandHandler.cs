using App.BuildingBlocks.Domain.Results;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;

internal class ConnectBankAccountToDebitWalletCommandHandler : ICommandHandler<ConnectBankAccountToDebitWalletCommand, Result<BankConnectionProcessInitiationSuccess, BankConnectionProcessInitiationError>>
{
    private readonly IDebitWalletRepository _debitWalletRepository;

    private readonly IBankConnectionProcessRepository _bankConnectionProcessRepository;

    private readonly IBankConnectionProcessInitiationService _bankConnectionProcessInitiationService;

    private readonly IBankConnectionProcessRedirectionService _bankConnectionProcessRedirectionService;

    public ConnectBankAccountToDebitWalletCommandHandler(
        IDebitWalletRepository debitWalletRepository,
        IBankConnectionProcessRepository bankConnectionProcessRepository,
        IBankConnectionProcessInitiationService bankConnectionProcessInitiationService,
        IBankConnectionProcessRedirectionService bankConnectionProcessRedirectionService)
    {
        _debitWalletRepository = debitWalletRepository;
        _bankConnectionProcessRepository = bankConnectionProcessRepository;
        _bankConnectionProcessInitiationService = bankConnectionProcessInitiationService;
        _bankConnectionProcessRedirectionService = bankConnectionProcessRedirectionService;
    }

    public async Task<Result<BankConnectionProcessInitiationSuccess, BankConnectionProcessInitiationError>> Handle(ConnectBankAccountToDebitWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await _debitWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));

        var bankConnectionProcess = await wallet.InitiateBankConnectionProcess(new BankId(command.BankId), _bankConnectionProcessInitiationService);
        var redirectionResult = await bankConnectionProcess.Redirect(_bankConnectionProcessRedirectionService);

        await _bankConnectionProcessRepository.AddAsync(bankConnectionProcess);

        if (redirectionResult.IsError && redirectionResult.Error == RedirectionError.ExternalProviderError)
        {
            return BankConnectionProcessInitiationError.ExternalProviderError;
        }

        return new BankConnectionProcessInitiationSuccess(bankConnectionProcess.Id.Value, redirectionResult.Success);
    }
}
