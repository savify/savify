using App.BuildingBlocks.Application.Outbox;

namespace App.Modules.Notifications.Infrastructure.Outbox;

public class Outbox : IOutbox
{
    private readonly NotificationsContext _notificationsContext;

    public Outbox(NotificationsContext notificationsContext)
    {
        _notificationsContext = notificationsContext;
    }

    public void Add(OutboxMessage message)
    {
        _notificationsContext.OutboxMessages?.Add(message);
    }
}
