using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Queries;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Banks.Application.Banks.Internal.GetBanks;

internal class GetBanksQueryHandler : IQueryHandler<GetBanksQuery, IList<BankDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetBanksQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IList<BankDto>> Handle(GetBanksQuery query, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT id, name, country_code AS countryCode, external_provider_name AS externalProviderName, status,
                       last_banks_synchronisation_process_id AS lastBanksSynchronisationProcessId, max_consent_days AS maxConsentDays,
                       is_regulated AS isRegulated, default_logo_url AS defaultLogoUrl, logo_url AS logoUrl, created_at AS createdAt, updated_at AS updatedAt
                   FROM {DatabaseConfiguration.Schema.Name}.banks
                   """;

        var pagedSqlQuery = new PagedSqlQueryBuilder()
            .WithSql(sql)
            .WithParametersFrom(query)
            .Build();

        var banks = await connection.QueryAsync<BankDto>(pagedSqlQuery.Sql, pagedSqlQuery.Parameters);

        return banks.ToList();
    }
}
