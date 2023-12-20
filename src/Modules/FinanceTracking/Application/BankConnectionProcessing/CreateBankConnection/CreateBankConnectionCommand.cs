using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.CreateBankConnection;

public class CreateBankConnectionCommand(Guid bankConnectionProcessId, string externalBankConnectionId)
    : CommandBase<EmptyResult<CreateBankConnectionError>>
{
    public Guid BankConnectionProcessId { get; } = bankConnectionProcessId;

    public string ExternalBankConnectionId { get; } = externalBankConnectionId;
}
