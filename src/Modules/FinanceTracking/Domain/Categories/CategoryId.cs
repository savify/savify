using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Categories;

public class CategoryId(Guid value) : TypedIdValueBase(value);
