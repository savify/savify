using App.BuildingBlocks.Application.Data;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Banks.Application.Banks.Public.GetBank;

internal class GetBankQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetBankQuery, BankDto?>
{
    public async Task<BankDto?> Handle(GetBankQuery query, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT id, name, country_code AS countryCode, default_logo_url AS defaultLogoUrl, logo_url AS logoUrl
                   FROM {DatabaseConfiguration.Schema.Name}.banks
                   WHERE id = @Id
                   """;

        return await connection.QuerySingleOrDefaultAsync<BankDto>(sql, new { query.Id });
    }
}
