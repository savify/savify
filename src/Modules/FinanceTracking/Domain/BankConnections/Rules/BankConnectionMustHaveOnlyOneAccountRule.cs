using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;

namespace App.Modules.FinanceTracking.Domain.BankConnections.Rules;

public class BankConnectionMustHaveOnlyOneAccountRule : IBusinessRule
{
    private readonly List<BankAccount> _accounts;

    public BankConnectionMustHaveOnlyOneAccountRule(List<BankAccount> accounts)
    {
        _accounts = accounts;
    }

    public bool IsBroken() => _accounts.Count != 1;

    public string MessageTemplate => "Bank connection has no bank accounts or more than one bank account";
}
