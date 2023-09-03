using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;

namespace App.Modules.Wallets.Domain.Wallets.BankAccountConnections;

public interface IBankAccountConnector
{
    public Task ConnectBankAccountToWallet(WalletId walletId, WalletType walletType, BankConnectionId bankConnectionId, BankAccountId bankAccountId);
}
