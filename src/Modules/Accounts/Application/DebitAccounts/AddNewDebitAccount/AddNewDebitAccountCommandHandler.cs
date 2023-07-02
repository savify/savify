using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;
using App.Modules.Accounts.Domain.Accounts.DebitAccounts;

namespace App.Modules.Accounts.Application.DebitAccounts.AddNewDebitAccount;

internal class AddNewDebitAccountCommandHandler : ICommandHandler<AddNewDebitAccountCommand, Guid>
{
    private readonly IDebitAccountRepository _debitAccountRepository;
    private readonly IAccountViewMetadataRepository _accountViewMetadataRepository;

    public AddNewDebitAccountCommandHandler(IDebitAccountRepository debitAccountRepository, IAccountViewMetadataRepository accountViewMetadataRepository)
    {
        _debitAccountRepository = debitAccountRepository;
        _accountViewMetadataRepository = accountViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewDebitAccountCommand request, CancellationToken cancellationToken)
    {
        var debitAccount = DebitAccount.AddNew(
            new Domain.Users.UserId(request.UserId),
            request.Title,
            Currency.From(request.Currency),
            request.Balance);

        await _debitAccountRepository.AddAsync(debitAccount);

        var viewMetadata = AccountViewMetadata.CreateDefaultForAccount(debitAccount.Id);
        await _accountViewMetadataRepository.AddAsync(viewMetadata);

        return debitAccount.Id.Value;
    }
}
