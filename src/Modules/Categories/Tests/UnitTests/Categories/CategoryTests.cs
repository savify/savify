using App.Modules.Categories.Domain.Categories;
using App.Modules.Categories.Domain.Categories.Events;
using App.Modules.Categories.Domain.Categories.Rules;

namespace App.Modules.Categories.UnitTests.Categories;

[TestFixture]
public class CategoryTests : UnitTestBase
{
    [Test]
    public void CreatingNewCategory_IsSuccessful()
    {
        var categoriesCounter = Substitute.For<ICategoriesCounter>();

        var category = Category.Create("external_id", "Category", CategoryType.Expense, categoriesCounter);

        var newCategoryCreatedDomainEvent = AssertPublishedDomainEvent<NewCategoryCreatedDomainEvent>(category);
        Assert.That(newCategoryCreatedDomainEvent.CategoryId, Is.EqualTo(category.Id));
        Assert.That(newCategoryCreatedDomainEvent.ExternalId, Is.EqualTo("external_id"));
    }

    [Test]
    public void CreatingNewCategory_WhenExternalIdIsNotUnique_BreaksExternalIdMustBeUniqueRule()
    {
        var categoriesCounter = Substitute.For<ICategoriesCounter>();
        categoriesCounter.CountWithExternalId("external_id").Returns(1);

        AssertBrokenRule<ExternalIdMustBeUniqueRule>(() =>
            Category.Create("external_id", "Category", CategoryType.Expense, categoriesCounter));
    }

    [Test]
    public void AddingChildCategory_IsSuccessful()
    {
        var categoriesCounter = Substitute.For<ICategoriesCounter>();
        var category = Category.Create("external_id", "Category", CategoryType.Expense, categoriesCounter);

        var childCategory = category.AddChild("child_external_id", "Child category", categoriesCounter, CategoryType.Expense);

        var newCategoryCreatedDomainEvent = AssertPublishedDomainEvent<NewCategoryCreatedDomainEvent>(childCategory);
        Assert.That(childCategory.ParentId, Is.EqualTo(category.Id));
        Assert.That(newCategoryCreatedDomainEvent.CategoryId, Is.EqualTo(childCategory.Id));
        Assert.That(newCategoryCreatedDomainEvent.ExternalId, Is.EqualTo("child_external_id"));
    }

    [Test]
    public void AddingChildCategory_WhenExternalIdIsNotUnique_BreaksExternalIdMustBeUniqueRule()
    {
        var categoriesCounter = Substitute.For<ICategoriesCounter>();
        var category = Category.Create("external_id", "Category", CategoryType.Expense, categoriesCounter);
        categoriesCounter.CountWithExternalId("child_external_id").Returns(1);

        AssertBrokenRule<ExternalIdMustBeUniqueRule>(() =>
            category.AddChild("child_external_id", "Child category", categoriesCounter, CategoryType.Expense));
    }
}
