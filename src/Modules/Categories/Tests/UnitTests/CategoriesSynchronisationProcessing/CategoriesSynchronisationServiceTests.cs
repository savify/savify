using App.Modules.Categories.Domain.Categories;
using App.Modules.Categories.Infrastructure.Domain.CategoriesSynchronisationProcessing;
using App.Modules.Categories.Infrastructure.Integrations.SaltEdge;
using App.Modules.Categories.Infrastructure.Integrations.SaltEdge.Categories;

namespace App.Modules.Categories.UnitTests.CategoriesSynchronisationProcessing;

[TestFixture]
public class CategoriesSynchronisationServiceTests : UnitTestBase
{
    [Test]
    public async Task SynchroniseAsync_WillCreateCategories_FromExternalCategories()
    {
        var externalCategories = new SaltEdgeCategories(
            new Dictionary<string, IList<string>>(),
            new Dictionary<string, IList<string>>
            {
                {"income", new List<string>{"paycheck", "bonus"}},
                {"shopping", new List<string>{"clothing"}},
                {"home", new List<string>{"rent"}}
            });

        var categoriesCounter = Substitute.For<ICategoriesCounter>();
        var existingCategory = Category.Create("home", "Home", CategoryType.Expense, categoriesCounter);

        var categoryRepository = Substitute.For<ICategoryRepository>();
        categoryRepository.GetAllAsync().Returns(new List<Category>
        {
            existingCategory
        });

        var saltEdgeIntegrationService = Substitute.For<ISaltEdgeIntegrationService>();
        saltEdgeIntegrationService.FetchCategoriesAsync().Returns(externalCategories);

        var service = new CategoriesSynchronisationService(
            categoryRepository,
            saltEdgeIntegrationService,
            categoriesCounter);

        await service.SynchroniseAsync();

        await saltEdgeIntegrationService.Received(1).FetchCategoriesAsync();
        await categoryRepository.Received(1).AddAsync(Arg.Is<Category>(c =>
            c.ExternalId == "paycheck" &&
            c.ParentId == null &&
            c.Type == CategoryType.Income));

        await categoryRepository.Received(1).AddAsync(Arg.Is<Category>(c =>
            c.ExternalId == "bonus" &&
            c.ParentId == null &&
            c.Type == CategoryType.Income));

        await categoryRepository.Received(1).AddAsync(Arg.Is<Category>(c =>
            c.ExternalId == "shopping" &&
            c.ParentId == null &&
            c.Type == CategoryType.Expense));

        await categoryRepository.Received(1).AddAsync(Arg.Is<Category>(c =>
            c.ExternalId == "clothing" &&
            c.ParentId!.GetType() == typeof(CategoryId) &&
            c.Type == CategoryType.Expense));

        await categoryRepository.DidNotReceive().AddAsync(Arg.Is<Category>(c =>
            c.ExternalId == "home" &&
            c.ParentId == null &&
            c.Type == CategoryType.Expense));

        await categoryRepository.Received(1).AddAsync(Arg.Is<Category>(c =>
            c.ExternalId == "rent" &&
            c.ParentId == existingCategory.Id &&
            c.Type == CategoryType.Expense));
    }
}
