using App.Modules.Accounts.Application.Contracts;

namespace App.Modules.Accounts.Application.DebitAccounts.GetDebitAccount;

public class GetDebitAccountQuery : QueryBase<DebitAccountDto>
{
    public Guid AccountId { get; }

    public GetDebitAccountQuery(Guid accountId)
    {
        AccountId = accountId;
    }
}
