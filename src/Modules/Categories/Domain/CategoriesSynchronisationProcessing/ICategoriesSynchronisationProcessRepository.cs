namespace App.Modules.Categories.Domain.CategoriesSynchronisationProcessing;

public interface ICategoriesSynchronisationProcessRepository
{
    Task AddAsync(CategoriesSynchronisationProcess categoriesSynchronisationProcess);

    Task<CategoriesSynchronisationProcess> GetByIdAsync(CategoriesSynchronisationProcessId id);
}
