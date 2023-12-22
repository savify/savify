using App.BuildingBlocks.Integration;
using App.Modules.Categories.IntegrationEvents;
using MediatR;

namespace App.Modules.Categories.Application.Categories.CreateCategory;

public class NewCategoryCreatedPublishNotificationHandler(IEventBus eventBus) : INotificationHandler<NewCategoryCreatedNotification>
{
    public Task Handle(NewCategoryCreatedNotification notification, CancellationToken cancellationToken)
    {
        return eventBus.Publish(new NewCategoryCreatedIntegrationEvent(
            notification.Id,
            notification.CorrelationId,
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.CategoryId.Value,
            notification.DomainEvent.ExternalId));
    }
}
