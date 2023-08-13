﻿using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;
using App.Modules.Accounts.Domain.Accounts.DebitAccounts;

namespace App.Modules.Accounts.Application.Accounts.DebitAccounts.AddNewDebitAccount;

internal class AddNewDebitAccountCommandHandler : ICommandHandler<AddNewDebitAccountCommand, Guid>
{
    private readonly IDebitAccountRepository _debitAccountRepository;
    private readonly IAccountViewMetadataRepository _accountViewMetadataRepository;

    public AddNewDebitAccountCommandHandler(IDebitAccountRepository debitAccountRepository, IAccountViewMetadataRepository accountViewMetadataRepository)
    {
        _debitAccountRepository = debitAccountRepository;
        _accountViewMetadataRepository = accountViewMetadataRepository;
    }

    public async Task<Guid> Handle(AddNewDebitAccountCommand command, CancellationToken cancellationToken)
    {
        var debitAccount = DebitAccount.AddNew(
            new Domain.Users.UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.Balance);

        await _debitAccountRepository.AddAsync(debitAccount);

        var viewMetadata = AccountViewMetadata.CreateDefaultForAccount(debitAccount.Id);
        await _accountViewMetadataRepository.AddAsync(viewMetadata);

        return debitAccount.Id.Value;
    }
}
