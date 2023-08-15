using App.BuildingBlocks.Application.Outbox;

namespace App.Modules.Wallets.Infrastructure.Outbox;

public class Outbox : IOutbox
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
