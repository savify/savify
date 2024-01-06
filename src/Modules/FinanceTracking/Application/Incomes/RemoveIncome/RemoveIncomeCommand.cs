using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Incomes.RemoveIncome;

public class RemoveIncomeCommand(Guid incomeId, Guid userId) : CommandBase
{
    public Guid IncomeId { get; } = incomeId;

    public Guid UserId { get; } = userId;
}
