using App.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.BuildingBlocks.Infrastructure.Data;

public class TypedIdValueConverter<TTypedIdValue>(ConverterMappingHints? mappingHints = null)
    : ValueConverter<TTypedIdValue, Guid>(id => id.Value, value => Create(value), mappingHints)
    where TTypedIdValue : TypedIdValueBase
{
    private static TTypedIdValue Create(Guid id) => Activator.CreateInstance(typeof(TTypedIdValue), id) as TTypedIdValue ?? throw new InvalidOperationException();
}
