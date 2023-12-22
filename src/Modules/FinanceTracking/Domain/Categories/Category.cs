namespace App.Modules.FinanceTracking.Domain.Categories;

public class Category(CategoryId id, string externalId)
{
    public CategoryId Id { get; private set; } = id;

    public string ExternalId { get; private set; } = externalId;
}
