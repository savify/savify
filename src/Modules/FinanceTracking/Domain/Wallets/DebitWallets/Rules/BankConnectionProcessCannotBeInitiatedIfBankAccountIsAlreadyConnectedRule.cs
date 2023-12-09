using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Rules;

public class BankConnectionProcessCannotBeInitiatedIfBankAccountIsAlreadyConnectedRule : IBusinessRule
{
    private readonly bool _hasConnectedBankAccount;

    public BankConnectionProcessCannotBeInitiatedIfBankAccountIsAlreadyConnectedRule(bool hasConnectedBankAccount)
    {
        _hasConnectedBankAccount = hasConnectedBankAccount;
    }

    public bool IsBroken() => _hasConnectedBankAccount;

    public string MessageTemplate => "Wallet has bank account already connected";
}
