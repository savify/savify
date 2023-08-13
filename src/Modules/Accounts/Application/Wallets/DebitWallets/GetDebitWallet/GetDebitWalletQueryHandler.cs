using App.BuildingBlocks.Application.Data;
using App.Modules.Wallets.Application.Wallets.WalletsViewMetadata;
using App.Modules.Wallets.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.GetDebitWallet;

internal class GetDebitWalletQueryHandler : IQueryHandler<GetDebitWalletQuery, DebitWalletDto?>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetDebitWalletQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<DebitWalletDto?> Handle(GetDebitWalletQuery query, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = "SELECT d.id, d.user_id AS userId, d.title, d.currency, d.balance, d.created_at AS createdAt, " +
                           "v.account_id AS accountId, v.color, v.icon, v.is_considered_in_total_balance AS isConsideredInTotalBalance " +
                           "FROM accounts.debit_accounts d " +
                           "INNER JOIN accounts.account_view_metadata v ON d.id = v.account_id " +
                           "WHERE d.id = @WalletId";

        var debitWallets = await connection.QueryAsync<DebitWalletDto, WalletViewMetadataDto, DebitWalletDto>(sql, (debitWallet, viewMetadata) =>
        {
            debitWallet.ViewMetadata = viewMetadata;
            
            return debitWallet;
        },
        new { query.WalletId },
        splitOn: "walletId");
        
        return debitWallets.SingleOrDefault();
    }
}
