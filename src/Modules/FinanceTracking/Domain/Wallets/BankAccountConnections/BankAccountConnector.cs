using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

namespace App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;

public class BankAccountConnector(
    IDebitWalletRepository debitWalletRepository,
    IBankConnectionRepository bankConnectionRepository)
    : IBankAccountConnector
{
    public async Task ConnectBankAccountToWallet(
        WalletId walletId,
        WalletType walletType,
        BankConnectionId bankConnectionId,
        BankAccountId bankAccountId)
    {
        if (walletType == WalletType.Debit)
        {
            var wallet = await debitWalletRepository.GetByIdAsync(walletId);
            var bankConnection = await bankConnectionRepository.GetByIdAsync(bankConnectionId);

            var bankAccount = bankConnection.GetBankAccountById(bankAccountId);

            wallet.ConnectBankAccount(bankConnectionId, bankAccountId, bankAccount.Balance, bankAccount.Currency);
            await debitWalletRepository.SaveAsync(wallet);
        }

        // TODO: handle credit wallets and throw exception in case wallet type is not debit or credit
    }
}
