using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Budgets;

public class BudgetId(Guid value) : TypedIdValueBase(value);
