using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;

namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.CreateBankConnection;

internal class CreateBankConnectionCommandHandler : ICommandHandler<CreateBankConnectionCommand, EmptyResult<CreateBankConnectionError>>
{
    private readonly IBankConnectionProcessRepository _bankConnectionProcessRepository;

    private readonly IBankConnectionProcessConnectionCreationService _connectionCreationService;

    private readonly IBankAccountConnector _bankAccountConnector;

    public CreateBankConnectionCommandHandler(
        IBankConnectionProcessRepository bankConnectionProcessRepository,
        IBankConnectionProcessConnectionCreationService connectionCreationService,
        IBankAccountConnector bankAccountConnector)
    {
        _bankConnectionProcessRepository = bankConnectionProcessRepository;
        _connectionCreationService = connectionCreationService;
        _bankAccountConnector = bankAccountConnector;
    }

    public async Task<EmptyResult<CreateBankConnectionError>> Handle(CreateBankConnectionCommand command, CancellationToken cancellationToken)
    {
        var bankConnectionProcess = await _bankConnectionProcessRepository.GetByIdAsync(new BankConnectionProcessId(command.BankConnectionProcessId));

        var result = await bankConnectionProcess.CreateConnection(command.ExternalBankConnectionId, _connectionCreationService, _bankAccountConnector);

        if (result.IsError && result.Error == CreateConnectionError.ExternalProviderError)
        {
            return CreateBankConnectionError.ExternalProviderError;
        }

        return Result.Success;
    }
}
