using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Categories.Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Categories.Infrastructure.Domain.Categories;

public class CategoryRepository(CategoriesContext categoriesContext) : ICategoryRepository
{
    public async Task AddAsync(Category category)
    {
        await categoriesContext.AddAsync(category);
    }

    public async Task<Category> GetByIdAsync(CategoryId id)
    {
        var category = await categoriesContext.Categories.SingleOrDefaultAsync(x => x.Id == id);

        if (category is null)
        {
            throw new NotFoundRepositoryException<Category>(id.Value);
        }

        return category;
    }

    public Task<List<Category>> GetAllAsync()
    {
        return categoriesContext.Categories.ToListAsync();
    }

    public Task<Category?> GetByExternalIdAsync(string externalId)
    {
        return categoriesContext.Categories.SingleOrDefaultAsync(x => x.ExternalId == externalId);
    }
}
