namespace App.Modules.Categories.Domain.Categories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);

    Task<Category> GetByIdAsync(CategoryId id);

    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByExternalIdAsync(string externalId);
}
