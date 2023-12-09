using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Rules;

public class CannotOperateOnBankConnectionProcessWithFinalStatusRule : IBusinessRule
{
    private readonly BankConnectionProcessStatus _status;

    public CannotOperateOnBankConnectionProcessWithFinalStatusRule(BankConnectionProcessStatus status)
    {
        _status = status;
    }

    public bool IsBroken() => _status.IsFinal;

    public string MessageTemplate => "Bank connection process was already finished";
}
