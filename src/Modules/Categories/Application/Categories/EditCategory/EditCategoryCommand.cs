using App.Modules.Categories.Application.Contracts;

namespace App.Modules.Categories.Application.Categories.EditCategory;

public class EditCategoryCommand(Guid categoryId, string? title, string? iconUrl) : CommandBase
{
    public Guid CategoryId { get; } = categoryId;

    public string? Title { get; } = title;

    public string? IconUrl { get; } = iconUrl;
}
