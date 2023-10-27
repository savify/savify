using App.BuildingBlocks.Domain.Results;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.Wallets.BankAccountConnections;

namespace App.Modules.Wallets.Application.BankConnectionProcessing.CreateBankConnection;

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
