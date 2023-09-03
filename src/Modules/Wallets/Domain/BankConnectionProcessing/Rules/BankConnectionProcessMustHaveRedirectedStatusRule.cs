using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Rules;

public class BankConnectionProcessMustHaveRedirectedStatusRule : IBusinessRule
{
    private readonly BankConnectionProcessStatus _status;

    public BankConnectionProcessMustHaveRedirectedStatusRule(BankConnectionProcessStatus status)
    {
        _status = status;
    }

    public bool IsBroken() => _status != BankConnectionProcessStatus.Redirected;

    public string MessageTemplate => "Bank connection process should have status 'Redirected'";
}
