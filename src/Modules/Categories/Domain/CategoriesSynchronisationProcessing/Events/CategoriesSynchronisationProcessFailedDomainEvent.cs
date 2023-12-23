using App.BuildingBlocks.Domain;

namespace App.Modules.Categories.Domain.CategoriesSynchronisationProcessing.Events;

public class CategoriesSynchronisationProcessFailedDomainEvent(CategoriesSynchronisationProcessId categoriesSynchronisationProcessId) : DomainEventBase
{
    public CategoriesSynchronisationProcessId CategoriesSynchronisationProcessId { get; } = categoriesSynchronisationProcessId;
}
