using App.Modules.Categories.Application.Configuration.Commands;
using App.Modules.Categories.Domain.CategoriesSynchronisationProcessing;

namespace App.Modules.Categories.Application.CategoriesSynchronisationProcessing.SynchroniseCategories;

internal class SynchroniseCategoriesCommandHandler(
    ICategoriesSynchronisationProcessRepository categoriesSynchronisationProcessRepository,
    ICategoriesSynchronisationService categoriesSynchronisationService)
    : ICommandHandler<SynchroniseCategoriesCommand, CategoriesSynchronisationResultDto>
{
    public async Task<CategoriesSynchronisationResultDto> Handle(SynchroniseCategoriesCommand command, CancellationToken cancellationToken)
    {
        var categoriesSynchronisationProcess = await CategoriesSynchronisationProcess.Start(categoriesSynchronisationService);

        await categoriesSynchronisationProcessRepository.AddAsync(categoriesSynchronisationProcess);

        return new CategoriesSynchronisationResultDto(categoriesSynchronisationProcess.Status.Value);
    }
}
