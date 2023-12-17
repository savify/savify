using App.BuildingBlocks.Domain;

namespace App.Modules.Categories.Domain.Categories.Events;

public class NewCategoryCreatedDomainEvent(CategoryId categoryId, string externalId) : DomainEventBase
{
    public CategoryId CategoryId { get; } = categoryId;

    public string ExternalId { get; } = externalId;
}
