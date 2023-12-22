namespace App.Modules.FinanceTracking.Domain.Categories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);

    Task<Category> GetByIdAsync(CategoryId id);

    Task<Category?> GetByExternalIdAsync(string externalId);
}
