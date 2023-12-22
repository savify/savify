using App.Modules.Categories.IntegrationEvents;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using MediatR;

namespace App.Modules.FinanceTracking.Application.Categories;

public class NewCategoryCreatedIntegrationEventHandler(ICommandScheduler commandScheduler) : INotificationHandler<NewCategoryCreatedIntegrationEvent>
{
    public Task Handle(NewCategoryCreatedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        return commandScheduler.EnqueueAsync(new AddCategoryCommand(
            @event.Id,
            @event.CorrelationId,
            @event.CategoryId,
            @event.ExternalCategoryId));
    }
}
