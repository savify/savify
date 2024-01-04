using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Expenses.Events;

public class ExpenseRemovedDomainEvent(ExpenseId expenseId) : DomainEventBase
{
    public ExpenseId ExpenseId { get; } = expenseId;
}
