using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;

internal class ConnectBankAccountToDebitWalletCommandHandler(
    IDebitWalletRepository debitWalletRepository,
    IBankConnectionProcessRepository bankConnectionProcessRepository,
    IBankConnectionProcessInitiationService bankConnectionProcessInitiationService,
    IBankConnectionProcessRedirectionService bankConnectionProcessRedirectionService)
    : ICommandHandler<ConnectBankAccountToDebitWalletCommand,
        Result<BankConnectionProcessInitiationSuccess, BankConnectionProcessInitiationError>>
{
    public async Task<Result<BankConnectionProcessInitiationSuccess, BankConnectionProcessInitiationError>> Handle(ConnectBankAccountToDebitWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await debitWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));

        var bankConnectionProcess = await wallet.InitiateBankConnectionProcess(new BankId(command.BankId), bankConnectionProcessInitiationService);
        var redirectionResult = await bankConnectionProcess.Redirect(bankConnectionProcessRedirectionService);

        await bankConnectionProcessRepository.AddAsync(bankConnectionProcess);

        if (redirectionResult.IsError && redirectionResult.Error == RedirectionError.ExternalProviderError)
        {
            return BankConnectionProcessInitiationError.ExternalProviderError;
        }

        return new BankConnectionProcessInitiationSuccess(bankConnectionProcess.Id.Value, redirectionResult.Success);
    }
}
