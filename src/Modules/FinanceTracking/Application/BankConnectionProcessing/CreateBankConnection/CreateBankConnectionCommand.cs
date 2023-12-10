using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.CreateBankConnection;

public class CreateBankConnectionCommand : CommandBase<EmptyResult<CreateBankConnectionError>>
{
    public Guid BankConnectionProcessId { get; }

    public string ExternalBankConnectionId { get; }

    public CreateBankConnectionCommand(Guid bankConnectionProcessId, string externalBankConnectionId)
    {
        BankConnectionProcessId = bankConnectionProcessId;
        ExternalBankConnectionId = externalBankConnectionId;
    }
}
