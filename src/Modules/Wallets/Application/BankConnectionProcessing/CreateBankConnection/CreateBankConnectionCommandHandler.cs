using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.Wallets.BankAccountConnections;

namespace App.Modules.Wallets.Application.BankConnectionProcessing.CreateBankConnection;

internal class CreateBankConnectionCommandHandler : ICommandHandler<CreateBankConnectionCommand, Result>
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

    public async Task<Result> Handle(CreateBankConnectionCommand command, CancellationToken cancellationToken)
    {
        var bankConnectionProcess = await _bankConnectionProcessRepository.GetByIdAsync(new BankConnectionProcessId(command.BankConnectionProcessId));

        await bankConnectionProcess.CreateConnection(command.ExternalBankConnectionId, _connectionCreationService, _bankAccountConnector);

        return Result.Success;
    }
}
