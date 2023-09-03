using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;
using App.Modules.Wallets.Domain.Wallets.CreditWallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets;

namespace App.Modules.Wallets.Domain.Wallets.BankAccountConnections;

public class BankAccountConnector
{
    private readonly IDebitWalletRepository _debitWalletRepository;

    private readonly ICreditWalletRepository _creditWalletRepository;

    private readonly IBankConnectionRepository _bankConnectionRepository;

    public BankAccountConnector(
        IDebitWalletRepository debitWalletRepository,
        ICreditWalletRepository creditWalletRepository,
        IBankConnectionRepository bankConnectionRepository)
    {
        _debitWalletRepository = debitWalletRepository;
        _creditWalletRepository = creditWalletRepository;
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

            wallet.ConnectBankAccount(bankConnectionId, bankAccountId, bankAccount.Amount, bankAccount.Currency);
        }

        // TODO: handle credit wallets and throw exception in case wallet type is not debit or credit
    }
}
