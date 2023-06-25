using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.CashAccounts;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Application.CashAccounts.AddNewCashAccount;

internal class AddNewCashAccountCommandHandler : ICommandHandler<AddNewCashAccountCommand, Guid>
{
    private readonly ICashAccountRepository _repository;

    public AddNewCashAccountCommandHandler(ICashAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddNewCashAccountCommand request, CancellationToken cancellationToken)
    {
        var cashAccount = CashAccount.AddNew(
            new UserId(request.UserId),
            request.Title,
            Currency.From(request.Currency),
            request.Balance);

        await _repository.AddAsync(cashAccount);

        return cashAccount.Id.Value;
    }
}
