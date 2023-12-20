using App.BuildingBlocks.Application.Data;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Banks.Application.Banks.Internal.GetBank;

internal class GetBankQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetBankQuery, BankDto?>
{
    public async Task<BankDto?> Handle(GetBankQuery query, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT id, name, country_code AS countryCode, external_provider_name AS externalProviderName, status,
                       last_banks_synchronisation_process_id AS lastBanksSynchronisationProcessId, max_consent_days AS maxConsentDays,
                       is_regulated AS isRegulated, default_logo_url AS defaultLogoUrl, logo_url AS logoUrl, created_at AS createdAt, updated_at AS updatedAt
                   FROM {DatabaseConfiguration.Schema.Name}.banks
                   WHERE id = @Id
                   """;

        return await connection.QuerySingleOrDefaultAsync<BankDto>(sql, new { query.Id });
    }
}
