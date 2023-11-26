using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BuildingBlocks.Infrastructure.Data.Postgres;

public static class HasPostgresArrayConversionExtensions
{
    public static PropertyBuilder<ICollection<T>> HasPostgresArrayConversion<T>(this PropertyBuilder<ICollection<T>> builder) => builder.HasConversion(new NpgsqlICollectionConverter<T>());
}
