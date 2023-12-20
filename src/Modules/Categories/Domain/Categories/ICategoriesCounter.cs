namespace App.Modules.Categories.Domain.Categories;

public interface ICategoriesCounter
{
    int CountWithExternalId(string externalId);
}
