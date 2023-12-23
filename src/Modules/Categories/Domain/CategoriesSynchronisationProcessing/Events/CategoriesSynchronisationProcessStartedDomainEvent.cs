using App.BuildingBlocks.Domain;

namespace App.Modules.Categories.Domain.CategoriesSynchronisationProcessing.Events;

public class CategoriesSynchronisationProcessStartedDomainEvent(CategoriesSynchronisationProcessId categoriesSynchronisationProcessId) : DomainEventBase
{
    public CategoriesSynchronisationProcessId CategoriesSynchronisationProcessId { get; } = categoriesSynchronisationProcessId;
}
