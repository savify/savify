using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.Accounts;
using App.Modules.Wallets.Domain.Accounts.AccountViewMetadata;
using App.Modules.Wallets.Domain.Accounts.CashAccounts;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Accounts.Application.Accounts.CashAccounts.AddNewCashAccount;

internal class AddNewCashAccountCommandHandler : ICommandHandler<AddNewCashAccountCommand, Guid>
{
    private readonly ICashAccountRepository _cashAccountRepository;
    private readonly IWalletViewMetadataRepository _walletViewMetadataRepository;

    public AddNewCashAccountCommandHandler(ICashAccountRepository cashAccountRepository, IWalletViewMetadataRepository walletViewMetadataRepository)
    {
        _cashAccountRepository = cashAccountRepository;
        _walletViewMetadataRepository = walletViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewCashAccountCommand command, CancellationToken cancellationToken)
    {
        var cashAccount = CashAccount.AddNew(
            new UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.Balance);

        await _cashAccountRepository.AddAsync(cashAccount);

        var viewMetadata = WalletViewMetadata.CreateDefaultForAccount(cashAccount.Id);
        await _walletViewMetadataRepository.AddAsync(viewMetadata);

        return cashAccount.Id.Value;
    }
}
