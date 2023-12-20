using App.BuildingBlocks.Domain;

namespace App.Modules.Categories.Domain.Categories;

public class CategoryId(Guid value) : TypedIdValueBase(value);
