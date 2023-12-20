using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.GetBankConnectionProcess;

public class GetBankConnectionProcessQuery(Guid bankConnectionProcessId) : QueryBase<BankConnectionProcessDto?>
{
    public Guid BankConnectionProcessId { get; } = bankConnectionProcessId;
}
