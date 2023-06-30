using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;
using App.Modules.Accounts.Domain.Accounts.CashAccounts;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Application.CashAccounts.AddNewCashAccount;

internal class AddNewCashAccountCommandHandler : ICommandHandler<AddNewCashAccountCommand, Guid>
{
    private readonly ICashAccountRepository _cashAccountRepository;
    private readonly IAccountViewMetadataRepository _accountViewMetadataRepository;

    public AddNewCashAccountCommandHandler(ICashAccountRepository cashAccountRepository, IAccountViewMetadataRepository accountViewMetadataRepository)
    {
        _cashAccountRepository = cashAccountRepository;
        _accountViewMetadataRepository = accountViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewCashAccountCommand request, CancellationToken cancellationToken)
    {
        var cashAccount = CashAccount.AddNew(
            new UserId(request.UserId),
            request.Title,
            Currency.From(request.Currency),
            request.Balance);

        await _cashAccountRepository.AddAsync(cashAccount);

        var viewMetadata = AccountViewMetadata.CreateDefaultForAccount(cashAccount.Id);
        await _accountViewMetadataRepository.AddAsync(viewMetadata);

        return cashAccount.Id.Value;
    }
}
