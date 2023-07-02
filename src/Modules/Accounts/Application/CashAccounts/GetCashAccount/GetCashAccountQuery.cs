using App.Modules.Accounts.Application.Contracts;

namespace App.Modules.Accounts.Application.CashAccounts.GetCashAccount;
public class GetCashAccountQuery : QueryBase<CashAccountDto>
{
    public Guid AccountId { get; }

    public GetCashAccountQuery(Guid accountId)
    {
        AccountId = accountId;
    }
}
