using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Rules;

public class BankConnectionProcessCannotMakeRedirectionWhenRedirectUrlIsExpiredRule : IBusinessRule
{
    private readonly BankConnectionProcessStatus _status;

    public BankConnectionProcessCannotMakeRedirectionWhenRedirectUrlIsExpiredRule(BankConnectionProcessStatus status)
    {
        _status = status;
    }

    public bool IsBroken() => _status == BankConnectionProcessStatus.RedirectUrlExpired;

    public string MessageTemplate => "Redirect URL was expired";
}
