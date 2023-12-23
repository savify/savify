using App.Modules.Categories.Application.Contracts;

namespace App.Modules.Categories.Application.Categories.CreateCategory;

public class CreateCategoryCommand(string externalId, string title, string type) : CommandBase<Guid>
{
    public string ExternalId { get; } = externalId;

    public string Title { get; } = title;

    public string Type { get; } = type;
}
