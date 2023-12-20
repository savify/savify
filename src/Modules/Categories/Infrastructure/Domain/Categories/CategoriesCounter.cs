using App.BuildingBlocks.Application.Data;
using App.Modules.Categories.Application.Configuration.Data;
using App.Modules.Categories.Domain.Categories;
using Dapper;

namespace App.Modules.Categories.Infrastructure.Domain.Categories;

public class CategoriesCounter(ISqlConnectionFactory sqlConnectionFactory) : ICategoriesCounter
{
    public int CountWithExternalId(string externalId)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"SELECT COUNT(*) FROM {DatabaseConfiguration.Schema.Name}.categories WHERE external_id = @externalId";

        return connection.QuerySingle<int>(sql, new { externalId });
    }
}
