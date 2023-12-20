using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;

namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.CreateBankConnection;

internal class CreateBankConnectionCommandHandler(
    IBankConnectionProcessRepository bankConnectionProcessRepository,
    IBankConnectionProcessConnectionCreationService connectionCreationService,
    IBankAccountConnector bankAccountConnector)
    : ICommandHandler<CreateBankConnectionCommand, EmptyResult<CreateBankConnectionError>>
{
    public async Task<EmptyResult<CreateBankConnectionError>> Handle(CreateBankConnectionCommand command, CancellationToken cancellationToken)
    {
        var bankConnectionProcess = await bankConnectionProcessRepository.GetByIdAsync(new BankConnectionProcessId(command.BankConnectionProcessId));

        var result = await bankConnectionProcess.CreateConnection(command.ExternalBankConnectionId, connectionCreationService, bankAccountConnector);

        if (result.IsError && result.Error == CreateConnectionError.ExternalProviderError)
        {
            return CreateBankConnectionError.ExternalProviderError;
        }

        return Result.Success;
    }
}
