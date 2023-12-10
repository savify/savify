using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;

namespace App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;

public interface IBankAccountConnector
{
    public Task ConnectBankAccountToWallet(WalletId walletId, WalletType walletType, BankConnectionId bankConnectionId, BankAccountId bankAccountId);
}
