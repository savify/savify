using App.BuildingBlocks.Application.Data;
using App.Modules.Wallets.Application.Wallets.WalletsViewMetadata;
using App.Modules.Wallets.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.GetCashWallet;

internal class GetCashWalletQueryHandler : IQueryHandler<GetCashWalletQuery, CashWalletDto?>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    
    public GetCashWalletQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<CashWalletDto?> Handle(GetCashWalletQuery query, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.GetOpenConnection();
    
        const string sql = "SELECT c.id, c.user_id AS userId, c.title, c.currency, c.balance, c.created_at AS createdAt, " +
                           "v.wallet_id AS walletId, v.color, v.icon, v.is_considered_in_total_balance AS isConsideredInTotalBalance " +
                           "FROM wallets.cash_wallets c " +
                           "INNER JOIN wallets.wallet_view_metadata v ON c.id = v.wallet_id " +
                           "WHERE id = @WalletId";
    
        var cashWallets = await connection.QueryAsync<CashWalletDto, WalletViewMetadataDto, CashWalletDto>(sql, (cashWallet, viewMetadata) =>
        {
            cashWallet.ViewMetadata = viewMetadata;
            
            return cashWallet;
        },
        new { query.WalletId },
        splitOn: "walletId");

        return cashWallets.SingleOrDefault();
    }
}
