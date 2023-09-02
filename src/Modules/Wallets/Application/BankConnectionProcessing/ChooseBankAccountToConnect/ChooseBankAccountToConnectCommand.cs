using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.BankConnectionProcessing.ChooseBankAccountToConnect;

public class ChooseBankAccountToConnectCommand : CommandBase<Result>
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
