using App.BuildingBlocks.Application.Data;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.FinanceTracking.Application.UserTags.GetUserTags;

internal class GetUserTagsQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetUserTagsQuery, UserTagsDto?>
{
    public async Task<UserTagsDto?> Handle(GetUserTagsQuery request, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateNewConnection();

        var sql = $"""
                   SELECT ut.tags as Values
                   FROM {DatabaseConfiguration.Schema.Name}.user_tags ut
                   WHERE ut.user_id = @UserId
                   """;

        var userTags = await connection.QuerySingleOrDefaultAsync<UserTagsDto>(sql, new { request.UserId });

        return userTags;
    }
}
