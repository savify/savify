using App.BuildingBlocks.Infrastructure.Outbox;

namespace App.Modules.UserAccess.Infrastructure.Outbox;

public class Outbox : IOutbox<UserAccessContext>
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
}
