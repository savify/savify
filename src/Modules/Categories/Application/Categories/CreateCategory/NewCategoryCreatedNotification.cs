using App.BuildingBlocks.Application.Events;
using App.Modules.Categories.Domain.Categories.Events;
using Newtonsoft.Json;

namespace App.Modules.Categories.Application.Categories.CreateCategory;

[method: JsonConstructor]
public class NewCategoryCreatedNotification(Guid id, Guid correlationId, NewCategoryCreatedDomainEvent domainEvent)
    : DomainEventNotificationBase<NewCategoryCreatedDomainEvent>(id, correlationId, domainEvent);
