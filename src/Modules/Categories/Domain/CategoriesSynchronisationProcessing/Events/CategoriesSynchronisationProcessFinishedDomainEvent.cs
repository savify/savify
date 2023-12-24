using App.BuildingBlocks.Domain;

namespace App.Modules.Categories.Domain.CategoriesSynchronisationProcessing.Events;

public class CategoriesSynchronisationProcessFinishedDomainEvent(CategoriesSynchronisationProcessId categoriesSynchronisationProcessId) : DomainEventBase
{
    public CategoriesSynchronisationProcessId CategoriesSynchronisationProcessId { get; } = categoriesSynchronisationProcessId;
}
