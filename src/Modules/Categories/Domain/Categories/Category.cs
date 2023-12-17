using App.BuildingBlocks.Domain;
using App.Modules.Categories.Domain.Categories.Events;
using App.Modules.Categories.Domain.Categories.Rules;

namespace App.Modules.Categories.Domain.Categories;

public class Category : Entity, IAggregateRoot
{
    public CategoryId Id { get; private set; }

    public string ExternalId { get; private set; }

    private string _title;

    private CategoryType _type;

    private CategoryId? _parentId = null;

    private Url? _iconUrl = null;

    public static Category Create(string externalId, string title, CategoryType type, ICategoriesCounter categoriesCounter, CategoryId? parentId = null, Url? iconUrl = null)
    {
        return new Category(externalId, title, type, categoriesCounter, parentId, iconUrl);
    }

    public Category AddChild(string externalId, string title, ICategoriesCounter categoriesCounter, CategoryType type, Url? iconUrl = null)
    {
        return Create(externalId, title, type, categoriesCounter, this.Id, iconUrl);
    }

    private Category(string externalId, string title, CategoryType type, ICategoriesCounter categoriesCounter, CategoryId? parentId = null, Url? iconUrl = null)
    {
        CheckRules(new ExternalIdMustBeUniqueRule(externalId, categoriesCounter));

        Id = new CategoryId(Guid.NewGuid());
        ExternalId = externalId;

        _title = title;
        _type = type;
        _parentId = parentId;
        _iconUrl = iconUrl;

        AddDomainEvent(new NewCategoryCreatedDomainEvent(Id, externalId));
    }

    private Category() { }
}
