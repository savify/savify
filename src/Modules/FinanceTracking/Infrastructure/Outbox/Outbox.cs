using App.BuildingBlocks.Infrastructure.Outbox;

namespace App.Modules.FinanceTracking.Infrastructure.Outbox;

public class Outbox(FinanceTrackingContext financeTrackingContext) : IOutbox<FinanceTrackingContext>
{
    public void Add(OutboxMessage message)
    {
        financeTrackingContext.OutboxMessages.Add(message);
    }
}
