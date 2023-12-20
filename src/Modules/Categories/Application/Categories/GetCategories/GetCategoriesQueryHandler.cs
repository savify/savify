using App.BuildingBlocks.Application.Data;
using App.Modules.Categories.Application.Configuration.Data;
using App.Modules.Categories.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Categories.Application.Categories.GetCategories;

internal class GetCategoriesQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateNewConnection();

        var sql = $@"SELECT id, external_id AS externalId, parent_id AS parentId, title, type, icon_url AS iconUrl
                        FROM {DatabaseConfiguration.Schema.Name}.categories";

        return await connection.QueryAsync<CategoryDto>(sql);
    }
}
