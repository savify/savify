namespace App.Modules.Categories.Application.Categories.GetCategories;

public class CategoryDto
{
    public Guid Id { get; set; }

    public required string ExternalId { get; set; }

    public Guid? ParentId { get; set; }

    public required string Title { get; set; }

    public required string Type { get; set; }

    public string? IconUrl { get; set; }
}
