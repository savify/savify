using App.BuildingBlocks.Application.Data;
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
                   SELECT t.id, t.source_wallet_id as sourceWalletId, t.target_wallet_id as targetWalletId, t.amount, t.currency, t.made_on as madeOn, t.comment, t.tags
                   FROM {DatabaseConfiguration.Schema.Name}.transfers t 
                   WHERE t.id = @TransferId
                   """;

        var transfer = await connection.QuerySingleOrDefaultAsync<TransferDto>(sql, new { query.TransferId });

        return transfer;
    }
}
