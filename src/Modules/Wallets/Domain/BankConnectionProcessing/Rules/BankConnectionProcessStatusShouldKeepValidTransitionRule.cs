using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Rules;

public class BankConnectionProcessStatusShouldKeepValidTransitionRule : IBusinessRule
{
    private readonly BankConnectionProcessStatus _status;

    private readonly BankConnectionProcessStatus _newStatus;

    public BankConnectionProcessStatusShouldKeepValidTransitionRule(BankConnectionProcessStatus status, BankConnectionProcessStatus newStatus)
    {
        _status = status;
        _newStatus = newStatus;
    }

    public bool IsBroken() => !_status.IsStatusTransitionValid(_newStatus);

    public string MessageTemplate => "Status transition from '{0}' to '{1}' is not valid";

    public object[] MessageArguments => new object[] { _status.Value, _newStatus.Value };
}
