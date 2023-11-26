using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.ValueConversion;

namespace App.BuildingBlocks.Infrastructure.Data.Postgres;

public class NpgsqlICollectionConverter<T> : ValueConverter<ICollection<T>, List<T>>, INpgsqlArrayConverter
{
    public NpgsqlICollectionConverter() : base(collection => new List<T>(collection), list => list)
    { }

    public ValueConverter ElementConverter => new ValueConverter<T, T>(x => x, x => x);
}
