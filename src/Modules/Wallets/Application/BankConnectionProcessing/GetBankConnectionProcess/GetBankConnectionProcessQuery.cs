using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.BankConnectionProcessing.GetBankConnectionProcess;

public class GetBankConnectionProcessQuery : QueryBase<BankConnectionProcessDto?>
{
    public Guid BankConnectionProcessId { get; }

    public GetBankConnectionProcessQuery(Guid bankConnectionProcessId)
    {
        BankConnectionProcessId = bankConnectionProcessId;
    }
}
