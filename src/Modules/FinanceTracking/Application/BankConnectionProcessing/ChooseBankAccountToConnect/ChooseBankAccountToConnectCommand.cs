using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.ChooseBankAccountToConnect;

public class ChooseBankAccountToConnectCommand(Guid bankConnectionProcessId, Guid userId, Guid bankAccountId) : CommandBase
{
    public Guid BankConnectionProcessId { get; } = bankConnectionProcessId;

    public Guid UserId { get; } = userId;

    public Guid BankAccountId { get; } = bankAccountId;
}
