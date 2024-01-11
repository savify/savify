using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Queries;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Application.Configuration.Queries;
using App.Modules.Banks.Domain.Banks;
using Dapper;

namespace App.Modules.Banks.Application.Banks.Public.GetBanks;

internal class GetBanksQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetBanksQuery, IEnumerable<BankDto>>
{
    public async Task<IEnumerable<BankDto>> Handle(GetBanksQuery query, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT id, name, country_code AS countryCode, (status = '{BankStatus.Beta.Value}') AS isBeta,
                          default_logo_url AS defaultLogoUrl, logo_url AS logoUrl
                   FROM {DatabaseConfiguration.Schema.Name}.banks
                   WHERE status != '{BankStatus.Disabled.Value}'
                   """;

        var pagedSqlQuery = new PagedSqlQueryBuilder()
            .WithSql(sql)
            .WithParametersFrom(query)
            .Build();

        return await connection.QueryAsync<BankDto>(pagedSqlQuery.Sql, pagedSqlQuery.Parameters);
    }
}
