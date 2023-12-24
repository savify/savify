using App.BuildingBlocks.Domain;
using App.Modules.Categories.Domain.Categories.Events;
using App.Modules.Categories.Domain.Categories.Rules;

namespace App.Modules.Categories.Domain.Categories;

public class Category : Entity, IAggregateRoot
{
    public CategoryId Id { get; private set; }

    public string ExternalId { get; private set; }

    public CategoryId? ParentId { get; private set; }

    private string _title;

    private CategoryType _type;

    public CategoryType Type => _type;

    private Url? _iconUrl;

    public static Category Create(string externalId, string title, CategoryType type, ICategoriesCounter categoriesCounter, CategoryId? parentId = null, Url? iconUrl = null)
    {
        return new Category(externalId, title, type, categoriesCounter, parentId, iconUrl);
    }

    public Category AddChild(string externalId, string title, CategoryType type, ICategoriesCounter categoriesCounter, Url? iconUrl = null)
    {
        return Create(externalId, title, type, categoriesCounter, this.Id, iconUrl);
    }

    public void Edit(string? newTitle, Url? newIconUrl)
    {
        _title = newTitle ?? _title;
        _iconUrl = newIconUrl ?? _iconUrl;

        AddDomainEvent(new CategoryEditedDomainEvent(Id, _title, _iconUrl));
    }

    private Category(string externalId, string title, CategoryType type, ICategoriesCounter categoriesCounter, CategoryId? parentId = null, Url? iconUrl = null)
    {
        CheckRules(new ExternalIdMustBeUniqueRule(externalId, categoriesCounter));

        Id = new CategoryId(Guid.NewGuid());
        ExternalId = externalId;
        ParentId = parentId;

        _title = title;
        _type = type;
        _iconUrl = iconUrl;

        AddDomainEvent(new NewCategoryCreatedDomainEvent(Id, externalId));
    }

    private Category() { }
}
