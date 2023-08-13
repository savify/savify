using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;
using App.Modules.Accounts.Domain.Accounts.CreditAccounts;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Application.Accounts.CreditAccounts.AddNewCreditAccount;

internal class AddNewCreditAccountCommandHandler : ICommandHandler<AddNewCreditAccountCommand, Guid>
{
    private readonly ICreditAccountsRepository _creditAccountRepository;
    private readonly IAccountViewMetadataRepository _accountViewMetadataRepository;

    public AddNewCreditAccountCommandHandler(ICreditAccountsRepository creditAccountRepository, IAccountViewMetadataRepository accountViewMetadataRepository)
    {
        _creditAccountRepository = creditAccountRepository;
        _accountViewMetadataRepository = accountViewMetadataRepository;
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

        var viewMetadata = AccountViewMetadata.CreateDefaultForAccount(account.Id);
        await _accountViewMetadataRepository.AddAsync(viewMetadata);

        return account.Id.Value;
    }
}
