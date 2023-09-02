using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets.BankAccountConnection;

namespace App.Modules.Wallets.Application.BankConnectionProcessing.ChooseBankAccountToConnect;

public class ChooseBankAccountToConnectCommandHandler : ICommandHandler<ChooseBankAccountToConnectCommand, Result>
{
    private readonly IBankConnectionProcessRepository _bankConnectionProcessRepository;

    private readonly BankAccountConnector _bankAccountConnector;

    public ChooseBankAccountToConnectCommandHandler(IBankConnectionProcessRepository bankConnectionProcessRepository, BankAccountConnector bankAccountConnector)
    {
        _bankConnectionProcessRepository = bankConnectionProcessRepository;
        _bankAccountConnector = bankAccountConnector;
    }

    public async Task<Result> Handle(ChooseBankAccountToConnectCommand command, CancellationToken cancellationToken)
    {
        var bankConnectionProcess = await _bankConnectionProcessRepository.GetByIdAndUserIdAsync(
            new BankConnectionProcessId(command.BankConnectionProcessId), new UserId(command.UserId));

        await bankConnectionProcess.ChooseBankAccount(new BankAccountId(command.BankAccountId), _bankAccountConnector);

        return Result.Success;
    }
}
