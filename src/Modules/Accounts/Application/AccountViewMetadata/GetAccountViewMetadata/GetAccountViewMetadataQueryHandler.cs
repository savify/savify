using App.BuildingBlocks.Application.Data;
using App.Modules.Accounts.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Accounts.Application.ViewMetadata.GetViewMetadata;

internal class GetAccountViewMetadataQueryHandler : IQueryHandler<GetAccountViewMetadataQuery, AccountViewMetadataDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAccountViewMetadataQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<AccountViewMetadataDto> Handle(GetAccountViewMetadataQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = "SELECT account_id, color, icon, is_considered_in_total_balance " +
                           "FROM accounts.account_view_metadata " +
                           "WHERE account_id = @AccountId";

        return await connection.QuerySingleOrDefaultAsync<AccountViewMetadataDto>(sql, new { request.AccountId });
    }
}
