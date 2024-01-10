using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Incomes.Events;

public class IncomeRemovedDomainEvent(IncomeId incomeId) : DomainEventBase
{
    public IncomeId IncomeId { get; } = incomeId;
}
