using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Rules;

public class BankConnectionProcessStatusShouldKeepValidTransitionRule : IBusinessRule
{
    private readonly BankConnectionProcessStatus _currentStatus;

    private readonly BankConnectionProcessStatus _newStatus;

    public BankConnectionProcessStatusShouldKeepValidTransitionRule(BankConnectionProcessStatus currentStatus, BankConnectionProcessStatus newStatus)
    {
        _currentStatus = currentStatus;
        _newStatus = newStatus;
    }

    public bool IsBroken() => !_currentStatus.IsStatusTransitionValid(_newStatus);

    public string MessageTemplate => "Status transition from '{0}' to '{1}' is not valid";

    public object[] MessageArguments => new object[] { _currentStatus.Value, _newStatus.Value };
}
