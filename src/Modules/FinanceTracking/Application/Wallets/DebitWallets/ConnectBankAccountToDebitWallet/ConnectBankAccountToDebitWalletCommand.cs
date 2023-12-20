using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;

public class ConnectBankAccountToDebitWalletCommand(Guid userId, Guid walletId, Guid bankId)
    : CommandBase<Result<BankConnectionProcessInitiationSuccess, BankConnectionProcessInitiationError>>
{
    public Guid UserId { get; } = userId;

    public Guid WalletId { get; } = walletId;

    public Guid BankId { get; } = bankId;
}
