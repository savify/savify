using App.BuildingBlocks.Integration;
using MediatR;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;

public class PasswordResetRequestedPublishNotificationHandler : INotificationHandler<PasswordResetRequestedNotification>
{
    private readonly IEventBus _eventBus;

    public PasswordResetRequestedPublishNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(PasswordResetRequestedNotification notification, CancellationToken cancellationToken)
    {
        
    }
}
