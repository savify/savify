using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.GetBankConnectionProcess;

public class GetBankConnectionProcessQuery : QueryBase<BankConnectionProcessDto?>
{
    public Guid BankConnectionProcessId { get; }

    public GetBankConnectionProcessQuery(Guid bankConnectionProcessId)
    {
        BankConnectionProcessId = bankConnectionProcessId;
    }
}
