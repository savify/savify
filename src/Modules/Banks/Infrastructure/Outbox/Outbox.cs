using App.BuildingBlocks.Application.Outbox;

namespace App.Modules.Banks.Infrastructure.Outbox;

public class Outbox : IOutbox
{
    private readonly BanksContext _banksContext;

    public Outbox(BanksContext banksContext)
    {
        _banksContext = banksContext;
    }

    public void Add(OutboxMessage message)
    {
        _banksContext.OutboxMessages?.Add(message);
    }
}
