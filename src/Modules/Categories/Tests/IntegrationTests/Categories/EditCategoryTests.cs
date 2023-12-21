using App.BuildingBlocks.Application.Exceptions;
using App.Modules.Categories.Application.Categories.CreateCategory;
using App.Modules.Categories.Application.Categories.EditCategory;
using App.Modules.Categories.Application.Categories.GetCategories;
using App.Modules.Categories.IntegrationTests.SeedWork;

namespace App.Modules.Categories.IntegrationTests.Categories;

[TestFixture]
public class EditCategoryTests : TestBase
{
    [Test]
    public async Task EditCategoryCommand_Tests()
    {
        var categoryId = await CategoriesModule.ExecuteCommandAsync(new CreateCategoryCommand(
            "external_id",
            "Category",
            "Expense"));

        await CategoriesModule.ExecuteCommandAsync(new EditCategoryCommand(
            categoryId,
            "New title",
            "https://new-icon-url.com"));

        var editedCategory = await GetEditedCategory(categoryId);

        Assert.That(editedCategory, Is.Not.Null);
        Assert.That(editedCategory!.Title, Is.EqualTo("New title"));
        Assert.That(editedCategory.IconUrl, Is.EqualTo("https://new-icon-url.com"));
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("Ne")]
    public void EditCategoryCommand_WhenTitleIsInvalid_ThrowsInvalidCommandException(string title)
    {
        var categoryId = Guid.NewGuid();

        Assert.That(() => CategoriesModule.ExecuteCommandAsync(new EditCategoryCommand(
            categoryId,
            title,
            "https://new-icon-url.com")), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("invalid_url")]
    public void EditCategoryCommand_WhenIconUrlIsInvalid_ThrowsInvalidCommandException(string iconUrl)
    {
        var categoryId = Guid.NewGuid();

        Assert.That(() => CategoriesModule.ExecuteCommandAsync(new EditCategoryCommand(
            categoryId,
            "New title",
            iconUrl)), Throws.TypeOf<InvalidCommandException>());
    }

    private async Task<CategoryDto?> GetEditedCategory(Guid categoryId)
    {
        var categories = await CategoriesModule.ExecuteQueryAsync(new GetCategoriesQuery());

        return categories.SingleOrDefault(c => c.Id == categoryId);
    }
}
