namespace App.Modules.Categories.Domain.CategoriesSynchronisationProcessing;

public record CategoriesSynchronisationProcessStatus(string Value)
{
    public static CategoriesSynchronisationProcessStatus Started => new(nameof(Started));

    public static CategoriesSynchronisationProcessStatus Finished => new(nameof(Finished));

    public static CategoriesSynchronisationProcessStatus Failed => new(nameof(Failed));
}
