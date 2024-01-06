using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Incomes.GetIncome;

public class GetIncomeQuery(Guid incomeId, Guid userId) : QueryBase<IncomeDto?>
{
    public Guid IncomeId { get; } = incomeId;

    public Guid UserId { get; } = userId;
}
