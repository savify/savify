using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.ChooseBankAccountToConnect;

public class ChooseBankAccountToConnectCommand : CommandBase
{
    public Guid BankConnectionProcessId { get; }

    public Guid UserId { get; }

    public Guid BankAccountId { get; }

    public ChooseBankAccountToConnectCommand(Guid bankConnectionProcessId, Guid userId, Guid bankAccountId)
    {
        BankConnectionProcessId = bankConnectionProcessId;
        UserId = userId;
        BankAccountId = bankAccountId;
    }
}
