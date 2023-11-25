using App.Modules.Banks.Application.Contracts;

namespace App.Modules.Banks.Application.Banks.Public.GetBank;

public class GetBankQuery : QueryBase<BankDto?>
{
    public Guid Id { get; }

    public GetBankQuery(Guid id)
    {
        Id = id;
    }
}
