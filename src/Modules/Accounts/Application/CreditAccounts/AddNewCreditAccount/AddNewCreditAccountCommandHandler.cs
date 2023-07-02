using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;
using App.Modules.Accounts.Domain.Accounts.CreditAccounts;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Application.CreditAccounts.AddNewCreditAccount;

public class AddNewCreditAccountCommandHandler : ICommandHandler<AddNewCreditAccountCommand, Guid>
{
    private readonly ICreditAccountsRepository _creditAccountRepository;
    private readonly IAccountViewMetadataRepository _accountViewMetadataRepository;

    public AddNewCreditAccountCommandHandler(ICreditAccountsRepository creditAccountRepository, IAccountViewMetadataRepository accountViewMetadataRepository)
    {
        _creditAccountRepository = creditAccountRepository;
        _accountViewMetadataRepository = accountViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewCreditAccountCommand request, CancellationToken cancellationToken)
    {
        var account = CreditAccount.AddNew(
            new UserId(request.UserId),
            request.Title,
            Currency.From(request.Currency),
            request.CreditLimit,
            request.AvailableBalance);

        await _creditAccountRepository.AddAsync(account);

        var viewMetadata = AccountViewMetadata.CreateDefaultForAccount(account.Id);
        await _accountViewMetadataRepository.AddAsync(viewMetadata);

        return account.Id.Value;
    }
}
