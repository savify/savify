using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Categories;

public class CategoryRepository(FinanceTrackingContext financeTrackingContext) : ICategoryRepository
{
    public async Task AddAsync(Category category)
    {
        await financeTrackingContext.AddAsync(category);
    }

    public async Task<Category> GetByIdAsync(CategoryId id)
    {
        var category = await financeTrackingContext.Categories.SingleOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            throw new NotFoundRepositoryException<Category>(id.Value);
        }

        return category;
    }

    public Task<Category?> GetByExternalIdAsync(string externalId)
    {
        return financeTrackingContext.Categories.SingleOrDefaultAsync(c => c.ExternalId == externalId);
    }
}
