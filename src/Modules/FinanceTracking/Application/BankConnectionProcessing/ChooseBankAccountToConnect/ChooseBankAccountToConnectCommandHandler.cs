using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;

namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.ChooseBankAccountToConnect;

internal class ChooseBankAccountToConnectCommandHandler(
    IBankConnectionProcessRepository bankConnectionProcessRepository,
    IBankAccountConnector bankAccountConnector)
    : ICommandHandler<ChooseBankAccountToConnectCommand>
{
    public async Task Handle(ChooseBankAccountToConnectCommand command, CancellationToken cancellationToken)
    {
        var bankConnectionProcess = await bankConnectionProcessRepository.GetByIdAndUserIdAsync(
            new BankConnectionProcessId(command.BankConnectionProcessId), new UserId(command.UserId));

        await bankConnectionProcess.ChooseBankAccount(new BankAccountId(command.BankAccountId), bankAccountConnector);
    }
}
