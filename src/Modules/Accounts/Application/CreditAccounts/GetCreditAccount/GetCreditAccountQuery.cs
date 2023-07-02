using App.Modules.Accounts.Application.Contracts;

namespace App.Modules.Accounts.Application.CreditAccounts.GetCreditAccount;
public class GetCreditAccountQuery : QueryBase<CreditAccountDto>
{
    public Guid AccountId { get; }

    public GetCreditAccountQuery(Guid accountId)
    {
        AccountId = accountId;
    }
}
