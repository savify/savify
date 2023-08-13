using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.Accounts;
using App.Modules.Wallets.Domain.Accounts.AccountViewMetadata;
using App.Modules.Wallets.Domain.Accounts.CreditAccounts;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Accounts.Application.Accounts.CreditAccounts.AddNewCreditAccount;

internal class AddNewCreditAccountCommandHandler : ICommandHandler<AddNewCreditAccountCommand, Guid>
{
    private readonly ICreditAccountsRepository _creditAccountRepository;
    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public AddNewCreditAccountCommandHandler(ICreditAccountsRepository creditAccountRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _creditAccountRepository = creditAccountRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewCreditAccountCommand command, CancellationToken cancellationToken)
    {
        var account = CreditAccount.AddNew(
            new UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.CreditLimit,
            command.AvailableBalance);

        await _creditAccountRepository.AddAsync(account);

        var viewMetadata = WalletViewMetadata.CreateDefaultForAccount(account.Id);
        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return account.Id.Value;
    }
}
