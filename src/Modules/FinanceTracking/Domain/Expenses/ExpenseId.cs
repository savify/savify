using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Expenses;

public class ExpenseId(Guid value) : TypedIdValueBase(value);
