using App.BuildingBlocks.Application.Outbox;

namespace App.Modules.Accounts.Infrastructure.Outbox;

public class Outbox : IOutbox
{
    private readonly AccountsContext _accountsContext;

    public Outbox(AccountsContext accountsContext)
    {
        _accountsContext = accountsContext;
    }

    public void Add(OutboxMessage message)
    {
        _accountsContext.OutboxMessages?.Add(message);
    }
}
