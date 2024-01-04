using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.GetBankConnectionProcess;

internal class GetBankConnectionProcessQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<GetBankConnectionProcessQuery, BankConnectionProcessDto?>
{
    public async Task<BankConnectionProcessDto?> Handle(GetBankConnectionProcessQuery query, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                  SELECT p.id, p.user_id AS userId, p.bank_id AS bankId, p.wallet_id AS walletId, p.wallet_type AS walletType,
                      p.status, p.redirect_url AS redirectUrl, p.redirect_url_expires_at AS redirectUrlExpiresAt,
                      p.initiated_at AS initiatedAt, p.updated_at AS updatedAt
                  FROM {DatabaseConfiguration.Schema.Name}.bank_connection_processes p
                  WHERE p.id = @Id
                  """;

        var bankConnectionProcess = await connection.QuerySingleOrDefaultAsync<BankConnectionProcessDto>(sql, new { Id = query.BankConnectionProcessId });

        if (bankConnectionProcess is not null && bankConnectionProcess.UserId != query.UserId)
        {
            throw new AccessDeniedException();
        }

        return bankConnectionProcess;
    }
}
