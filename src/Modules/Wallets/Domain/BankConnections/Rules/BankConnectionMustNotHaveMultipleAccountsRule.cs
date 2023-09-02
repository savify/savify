using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.BankConnections.Rules;

public class BankConnectionMustNotHaveMultipleAccountsRule : IBusinessRule
{
    private readonly bool _hasMultipleAccounts;

    public BankConnectionMustNotHaveMultipleAccountsRule(bool hasMultipleAccounts)
    {
        _hasMultipleAccounts = hasMultipleAccounts;
    }

    public bool IsBroken() => _hasMultipleAccounts;

    public string MessageTemplate => "Bank connection has more than one bank account";
}
