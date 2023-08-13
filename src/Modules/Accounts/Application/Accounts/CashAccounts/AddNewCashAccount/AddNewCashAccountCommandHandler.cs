using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;
using App.Modules.Accounts.Domain.Accounts.CashAccounts;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Application.Accounts.CashAccounts.AddNewCashAccount;

internal class AddNewCashAccountCommandHandler : ICommandHandler<AddNewCashAccountCommand, Guid>
{
    private readonly ICashAccountRepository _cashAccountRepository;
    private readonly IAccountViewMetadataRepository _accountViewMetadataRepository;

    public AddNewCashAccountCommandHandler(ICashAccountRepository cashAccountRepository, IAccountViewMetadataRepository accountViewMetadataRepository)
    {
        _cashAccountRepository = cashAccountRepository;
        _accountViewMetadataRepository = accountViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewCashAccountCommand command, CancellationToken cancellationToken)
    {
        var cashAccount = CashAccount.AddNew(
            new UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.Balance);

        await _cashAccountRepository.AddAsync(cashAccount);

        var viewMetadata = AccountViewMetadata.CreateDefaultForAccount(cashAccount.Id);
        await _accountViewMetadataRepository.AddAsync(viewMetadata);

        return cashAccount.Id.Value;
    }
}
