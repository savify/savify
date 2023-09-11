using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.Wallets.DebitWallets.Rules;

public class BankAccountCannotBeConnectedToWalletIfItAlreadyHasBankAccountConnectedRule : IBusinessRule
{
    private readonly bool _hasConnectedBankAccount;

    public BankAccountCannotBeConnectedToWalletIfItAlreadyHasBankAccountConnectedRule(bool hasConnectedBankAccount)
    {
        _hasConnectedBankAccount = hasConnectedBankAccount;
    }

    public bool IsBroken() => _hasConnectedBankAccount;

    public string MessageTemplate => "This wallet has bank account already connected";
}
