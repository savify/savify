using App.BuildingBlocks.Domain;

namespace App.Modules.Categories.Domain.Categories.Events;

public class CategoryEditedDomainEvent(CategoryId id, string? newTitle, Url? newIconUrl) : DomainEventBase
{
    public CategoryId CategoryId { get; } = id;

    public string? NewTitle { get; } = newTitle;

    public Url? NewIconUrl { get; } = newIconUrl;
}
