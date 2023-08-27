using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.DebitWallets;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;

public class ConnectBankAccountToDebitWalletCommandHandler : ICommandHandler<ConnectBankAccountToDebitWalletCommand, string>
{
    private readonly IDebitWalletRepository _debitWalletRepository;

    private readonly IBankConnectionProcessRepository _bankConnectionProcessRepository;

    private readonly IBankConnectionProcessInitiationService _bankConnectionProcessInitiationService;

    public ConnectBankAccountToDebitWalletCommandHandler(
        IDebitWalletRepository debitWalletRepository,
        IBankConnectionProcessRepository bankConnectionProcessRepository,
        IBankConnectionProcessInitiationService bankConnectionProcessInitiationService)
    {
        _debitWalletRepository = debitWalletRepository;
        _bankConnectionProcessRepository = bankConnectionProcessRepository;
        _bankConnectionProcessInitiationService = bankConnectionProcessInitiationService;
    }

    public async Task<string> Handle(ConnectBankAccountToDebitWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await _debitWalletRepository.GetByIdAndUserIdAsync(new WalletId(command.WalletId), new UserId(command.UserId));

        var bankConnectionProcess = await wallet.InitiateBankConnectionProcess(new BankId(command.BankId), _bankConnectionProcessInitiationService);

        await _bankConnectionProcessRepository.AddAsync(bankConnectionProcess);

        return "http://some-redirect-url.com";
    }
}
