using App.BuildingBlocks.Application.Data;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Configuration.Queries;
using App.Modules.FinanceTracking.Application.Contracts;
using Dapper;

namespace App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
public class GetUserTagsQuery(Guid userId) : QueryBase<UserTagsDto>
{
    public Guid UserId { get; } = userId;
}

public class UserTagsDto
{
    public required Guid UserId { get; init; }

    public required IEnumerable<string> Tags { get; init; }
}

internal class GetUserTagsQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetUserTagsQuery, UserTagsDto?>
{
    public async Task<UserTagsDto?> Handle(GetUserTagsQuery request, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateNewConnection();

        var sql = $"""
                   SELECT ut.user_id as userId, ut.tags
                   FROM {DatabaseConfiguration.Schema.Name}.user_tags ut
                   WHERE ut.user_id = @UserId
                   """;

        var userTags = await connection.QuerySingleOrDefaultAsync<UserTagsDto>(sql, new { request.UserId });

        return userTags;
    }
}
