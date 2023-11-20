using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Queries;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Banks.Application.Banks.Public.GetBanks;

internal class GetBanksQueryHandler : IQueryHandler<GetBanksQuery, IEnumerable<BankDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetBanksQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IEnumerable<BankDto>> Handle(GetBanksQuery query, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT id, name, country_code AS countryCode, default_logo_url AS defaultLogoUrl, logo_url AS logoUrl
                   FROM {DatabaseConfiguration.Schema.Name}.banks
                   """;

        var pagedSqlQuery = new PagedSqlQueryBuilder()
            .WithSql(sql)
            .WithParametersFrom(query)
            .Build();

        return await connection.QueryAsync<BankDto>(pagedSqlQuery.Sql, pagedSqlQuery.Parameters);
    }
}
