using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.FinanceTracking.Application.Transfers.GetTransfer;

internal class GetTransferQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetTransferQuery, TransferDto?>
{
    public async Task<TransferDto?> Handle(GetTransferQuery query, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT t.id, t.user_id as userId, t.source_wallet_id as sourceWalletId, t.target_wallet_id as targetWalletId, t.made_on as madeOn, t.comment, t.tags,
                        t.source_amount as SourceAmount, t.source_currency_code as SourceCurrency, t.target_amount as TargetAmount, t.target_currency_code as TargetCurrency, t.exchange_rate_rate as ExchangeRate
                   FROM {DatabaseConfiguration.Schema.Name}.transfers t
                   WHERE t.id = @TransferId
                   """;

        var transfer = await connection.QuerySingleOrDefaultAsync<TransferDto>(sql, new { query.TransferId });

        if (transfer is not null && transfer.UserId != query.UserId)
        {
            throw new AccessDeniedException();
        }

        return transfer;
    }
}
