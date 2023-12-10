using App.BuildingBlocks.Infrastructure.Outbox;

namespace App.Modules.FinanceTracking.Infrastructure.Outbox;

public class Outbox : IOutbox<FinanceTrackingContext>
{
    private readonly FinanceTrackingContext _financeTrackingContext;

    public Outbox(FinanceTrackingContext financeTrackingContext)
    {
        _financeTrackingContext = financeTrackingContext;
    }

    public void Add(OutboxMessage message)
    {
        _financeTrackingContext.OutboxMessages?.Add(message);
    }
}
