using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Rules;

public class RedirectUrlShouldBeExpiredRule : IBusinessRule
{
    private readonly DateTime? _expiresAt;

    public RedirectUrlShouldBeExpiredRule(DateTime? expiresAt)
    {
        _expiresAt = expiresAt;
    }

    public bool IsBroken() => _expiresAt is not null && _expiresAt > DateTime.UtcNow;

    public string MessageTemplate => "Redirect URL has not expired yet";
}
