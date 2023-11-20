using App.BuildingBlocks.Application.Data;
using App.Modules.Wallets.Application.Configuration.Data;
using App.Modules.Wallets.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Wallets.Application.BankConnectionProcessing.GetBankConnectionProcess;

internal class GetBankConnectionProcessQueryHandler : IQueryHandler<GetBankConnectionProcessQuery, BankConnectionProcessDto?>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetBankConnectionProcessQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<BankConnectionProcessDto?> Handle(GetBankConnectionProcessQuery query, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                  SELECT p.id, p.user_id AS userId, p.bank_id AS bankId, p.wallet_id AS walletId, p.wallet_type AS walletType,
                      p.status, p.redirect_url AS redirectUrl, p.redirect_url_expires_at AS redirectUrlExpiresAt,
                      p.initiated_at AS initiatedAt, p.updated_at AS updatedAt
                  FROM {DatabaseConfiguration.Schema.Name}.bank_connection_processes p
                  WHERE p.id = @Id
                  """;

        return await connection.QuerySingleOrDefaultAsync<BankConnectionProcessDto>(sql, new { Id = query.BankConnectionProcessId });
    }
}
