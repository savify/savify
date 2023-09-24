using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets.BankAccountConnections;

namespace App.Modules.Wallets.Application.BankConnectionProcessing.ChooseBankAccountToConnect;

internal class ChooseBankAccountToConnectCommandHandler : ICommandHandler<ChooseBankAccountToConnectCommand>
{
    private readonly IBankConnectionProcessRepository _bankConnectionProcessRepository;

    private readonly IBankAccountConnector _bankAccountConnector;

    public ChooseBankAccountToConnectCommandHandler(IBankConnectionProcessRepository bankConnectionProcessRepository, IBankAccountConnector bankAccountConnector)
    {
        _bankConnectionProcessRepository = bankConnectionProcessRepository;
        _bankAccountConnector = bankAccountConnector;
    }

    public async Task Handle(ChooseBankAccountToConnectCommand command, CancellationToken cancellationToken)
    {
        var bankConnectionProcess = await _bankConnectionProcessRepository.GetByIdAndUserIdAsync(
            new BankConnectionProcessId(command.BankConnectionProcessId), new UserId(command.UserId));

        await bankConnectionProcess.ChooseBankAccount(new BankAccountId(command.BankAccountId), _bankAccountConnector);
    }
}
