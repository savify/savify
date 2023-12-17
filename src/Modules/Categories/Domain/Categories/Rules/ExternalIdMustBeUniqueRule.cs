using App.BuildingBlocks.Domain;

namespace App.Modules.Categories.Domain.Categories.Rules;

public class ExternalIdMustBeUniqueRule(string externalId, ICategoriesCounter categoriesCounter) : IBusinessRule
{
    public bool IsBroken() => categoriesCounter.CountWithExternalId(externalId) > 0;

    public string MessageTemplate => "Category with external ID '{0}' already exists";
}
