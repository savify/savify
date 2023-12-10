using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;

public class ConnectBankAccountToDebitWalletCommand : CommandBase<Result<BankConnectionProcessInitiationSuccess, BankConnectionProcessInitiationError>>
{
    public Guid UserId { get; }

    public Guid WalletId { get; }

    public Guid BankId { get; }

    public ConnectBankAccountToDebitWalletCommand(Guid userId, Guid walletId, Guid bankId)
    {
        UserId = userId;
        WalletId = walletId;
        BankId = bankId;
    }
}
