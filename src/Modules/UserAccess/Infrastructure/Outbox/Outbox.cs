using App.BuildingBlocks.Application.Outbox;

namespace App.Modules.UserAccess.Infrastructure.Outbox;

public class Outbox : IOutbox
{
    private readonly UserAccessContext _userAccessContext;

    public Outbox(UserAccessContext userAccessContext)
    {
        _userAccessContext = userAccessContext;
    }

    public void Add(OutboxMessage message)
    {
        _userAccessContext.OutboxMessages?.Add(message);
    }

    public Task Save()
    {
        return Task.CompletedTask;
    }
}
