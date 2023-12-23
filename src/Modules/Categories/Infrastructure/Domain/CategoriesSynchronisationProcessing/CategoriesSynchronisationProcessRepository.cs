using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Categories.Domain.CategoriesSynchronisationProcessing;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Categories.Infrastructure.Domain.CategoriesSynchronisationProcessing;

public class CategoriesSynchronisationProcessRepository(CategoriesContext categoriesContext) : ICategoriesSynchronisationProcessRepository
{
    public async Task AddAsync(CategoriesSynchronisationProcess categoriesSynchronisationProcess)
    {
        await categoriesContext.AddAsync(categoriesSynchronisationProcess);
    }

    public async Task<CategoriesSynchronisationProcess> GetByIdAsync(CategoriesSynchronisationProcessId id)
    {
        var categoriesSynchronisationProcess = await categoriesContext.CategoriesSynchronisationProcesses.SingleOrDefaultAsync(b => b.Id == id);

        if (categoriesSynchronisationProcess is null)
        {
            throw new NotFoundRepositoryException<CategoriesSynchronisationProcess>(id.Value);
        }

        return categoriesSynchronisationProcess;
    }
}
