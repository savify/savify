using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Incomes;

public class IncomeId(Guid value) : TypedIdValueBase(value);
