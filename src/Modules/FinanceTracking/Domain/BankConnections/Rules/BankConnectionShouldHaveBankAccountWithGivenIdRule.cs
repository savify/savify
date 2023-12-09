using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;

namespace App.Modules.FinanceTracking.Domain.BankConnections.Rules;

public class BankConnectionShouldHaveBankAccountWithGivenIdRule : IBusinessRule
{
    private readonly List<BankAccount> _bankAccounts;

    private readonly BankAccountId _bankAccountId;

    public BankConnectionShouldHaveBankAccountWithGivenIdRule(List<BankAccount> bankAccounts, BankAccountId bankAccountId)
    {
        _bankAccounts = bankAccounts;
        _bankAccountId = bankAccountId;
    }

    public bool IsBroken() => !_bankAccounts.Exists(a => a.Id == _bankAccountId);

    public string MessageTemplate => "Bank connection does not have account with ID '{0}'";

    public object[] MessageArguments => new object[] { _bankAccountId.Value };
}
