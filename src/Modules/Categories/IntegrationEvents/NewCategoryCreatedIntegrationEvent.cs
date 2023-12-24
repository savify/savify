using App.BuildingBlocks.Integration;

namespace App.Modules.Categories.IntegrationEvents;

public class NewCategoryCreatedIntegrationEvent(
    Guid id,
    Guid correlationId,
    DateTime occurredOn,
    Guid categoryId,
    string externalCategoryId)
    : IntegrationEvent(id, correlationId, occurredOn)
{
    public Guid CategoryId { get; } = categoryId;

    public string ExternalCategoryId { get; } = externalCategoryId;
}
