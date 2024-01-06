using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Incomes;

public interface IIncomeRepository
{
    public Task AddAsync(Income income);

    public void Remove(Income income);

    public Task<Income> GetByIdAsync(IncomeId id);

    public Task<Income> GetByIdAndUserIdAsync(IncomeId id, UserId userId);
}
