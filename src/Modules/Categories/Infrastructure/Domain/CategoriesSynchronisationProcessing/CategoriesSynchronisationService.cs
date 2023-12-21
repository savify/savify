using App.Modules.Categories.Domain.Categories;
using App.Modules.Categories.Domain.CategoriesSynchronisationProcessing;
using App.Modules.Categories.Infrastructure.Integrations.SaltEdge;
using App.Modules.Categories.Infrastructure.Integrations.SaltEdge.Categories;

namespace App.Modules.Categories.Infrastructure.Domain.CategoriesSynchronisationProcessing;

public class CategoriesSynchronisationService(
    ICategoryRepository categoryRepository,
    ISaltEdgeIntegrationService saltEdgeIntegrationService,
    ICategoriesCounter categoriesCounter) : ICategoriesSynchronisationService
{
    public async Task SynchroniseAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        var externalCategories = await GetExternalCategories();

        await SynchroniseIncomeCategoriesAsync(externalCategories.Personal[SaltEdgeCategories.Income], categories);
        await SynchroniseExpenseCategoriesAsync(externalCategories, categories);
    }

    private async Task SynchroniseIncomeCategoriesAsync(IList<string> externalIncomeCategories, List<Category> categories)
    {
        foreach (var incomeCategoryId in externalIncomeCategories)
        {
            var category = categories.FirstOrDefault(c => c.ExternalId == incomeCategoryId);

            if (category is null)
            {
                category = Category.Create(
                    incomeCategoryId,
                    incomeCategoryId,
                    CategoryType.Income,
                    categoriesCounter);

                await categoryRepository.AddAsync(category);
            }
        }
    }

    private async Task SynchroniseExpenseCategoriesAsync(SaltEdgeCategories externalCategories, List<Category> categories)
    {
        foreach (var externalCategory in externalCategories.Personal)
        {
            var externalId = externalCategory.Key;
            if (externalId == SaltEdgeCategories.Income) continue;

            var category = categories.FirstOrDefault(c => c.ExternalId == externalId);
            if (category is null)
            {
                category = Category.Create(
                    externalId,
                    externalId,
                    CategoryType.Expense,
                    categoriesCounter);

                await categoryRepository.AddAsync(category);
                await AddChildCategoryAsync(category, externalCategory, categories);
            }
            else
            {
                await AddChildCategoryAsync(category, externalCategory, categories);
            }
        }
    }

    private async Task AddChildCategoryAsync(Category category, KeyValuePair<string, IList<string>> externalCategory, List<Category> categories)
    {
        foreach (var externalChildCategoryId in externalCategory.Value)
        {
            if (categories.Any(c => c.ExternalId == externalChildCategoryId)) continue;

            var childCategory = category.AddChild(
                externalChildCategoryId,
                externalChildCategoryId,
                CategoryType.Expense,
                categoriesCounter);

            await categoryRepository.AddAsync(childCategory);
        }
    }

    private async Task<SaltEdgeCategories> GetExternalCategories()
    {
        try
        {
            return await saltEdgeIntegrationService.FetchCategoriesAsync();
        }
        catch (SaltEdgeIntegrationException exception)
        {
            throw new CategoriesSynchronisationProcessException(exception.Message);
        }
    }
}
