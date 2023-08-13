using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.Accounts;
using App.Modules.Wallets.Domain.Accounts.AccountViewMetadata;
using App.Modules.Wallets.Domain.Accounts.DebitAccounts;

namespace App.Modules.Accounts.Application.Accounts.DebitAccounts.AddNewDebitAccount;

internal class AddNewDebitAccountCommandHandler : ICommandHandler<AddNewDebitAccountCommand, Guid>
{
    private readonly IDebitWalletRepository _debitWalletRepository;
    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public AddNewDebitAccountCommandHandler(IDebitWalletRepository debitWalletRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _debitWalletRepository = debitWalletRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewDebitAccountCommand command, CancellationToken cancellationToken)
    {
        var debitAccount = DebitWallet.AddNew(
            new Wallets.Domain.Users.UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.Balance);

        await _debitWalletRepository.AddAsync(debitAccount);

        var viewMetadata = WalletViewMetadata.CreateDefaultForAccount(debitAccount.Id);
        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return debitAccount.Id.Value;
    }
}
