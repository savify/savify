using App.BuildingBlocks.Domain.Results;
using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.BankConnectionProcessing.CreateBankConnection;

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
