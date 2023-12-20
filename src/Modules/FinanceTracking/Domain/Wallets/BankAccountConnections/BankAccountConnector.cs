using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

namespace App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;

public class BankAccountConnector : IBankAccountConnector
{
    private readonly IDebitWalletRepository _debitWalletRepository;

    private readonly IBankConnectionRepository _bankConnectionRepository;

    public BankAccountConnector(
        IDebitWalletRepository debitWalletRepository,
        IBankConnectionRepository bankConnectionRepository)
    {
        _debitWalletRepository = debitWalletRepository;
        _bankConnectionRepository = bankConnectionRepository;
    }

    public async Task ConnectBankAccountToWallet(
        WalletId walletId,
        WalletType walletType,
        BankConnectionId bankConnectionId,
        BankAccountId bankAccountId)
    {
        if (walletType == WalletType.Debit)
        {
            var wallet = await _debitWalletRepository.GetByIdAsync(walletId);
            var bankConnection = await _bankConnectionRepository.GetByIdAsync(bankConnectionId);

            var bankAccount = bankConnection.GetBankAccountById(bankAccountId);

            wallet.ConnectBankAccount(bankConnectionId, bankAccountId, bankAccount.Balance, bankAccount.Currency);
        }

        // TODO: handle credit wallets and throw exception in case wallet type is not debit or credit
    }
}
