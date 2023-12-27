using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Domain.Transfers.Rules;

public class TransferAmountMustBeBiggerThanZeroRule : IBusinessRule
{
    private readonly Money _amount;

    public TransferAmountMustBeBiggerThanZeroRule(Money amount)
    {
        _amount = amount;
    }

    public bool IsBroken()
    {
        return _amount.Amount <= 0;
    }

    public string MessageTemplate => "Transfer amount is smaller or equal to zero";
}
