using App.Modules.Banks.Application.Contracts;

namespace App.Modules.Banks.Application.Banks.Internal.GetBank;

public class GetBankQuery(Guid id) : QueryBase<BankDto?>
{
    public new Guid Id { get; } = id;
}
