using App.BuildingBlocks.Infrastructure.Outbox;

namespace App.Modules.UserAccess.Infrastructure.Outbox;

public class Outbox(UserAccessContext userAccessContext) : IOutbox<UserAccessContext>
{
    public void Add(OutboxMessage message)
    {
        userAccessContext.OutboxMessages.Add(message);
    }
}
