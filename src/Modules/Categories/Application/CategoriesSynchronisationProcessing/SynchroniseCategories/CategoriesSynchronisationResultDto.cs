namespace App.Modules.Categories.Application.CategoriesSynchronisationProcessing.SynchroniseCategories;

public class CategoriesSynchronisationResultDto(string status)
{
    public string Status { get; } = status;
}
