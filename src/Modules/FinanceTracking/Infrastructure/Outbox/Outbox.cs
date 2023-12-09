using App.BuildingBlocks.Infrastructure.Outbox;

namespace App.Modules.FinanceTracking.Infrastructure.Outbox;

public class Outbox : IOutbox<WalletsContext>
{
    private readonly WalletsContext _walletsContext;

    public Outbox(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public void Add(OutboxMessage message)
    {
        _walletsContext.OutboxMessages?.Add(message);
    }
}
