using App.BuildingBlocks.Infrastructure.Outbox;

namespace App.Modules.Banks.Infrastructure.Outbox;

public class Outbox(BanksContext banksContext) : IOutbox<BanksContext>
{
    public void Add(OutboxMessage message)
    {
        banksContext.OutboxMessages.Add(message);
    }
}
