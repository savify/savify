using App.BuildingBlocks.Application.Data;
using App.Modules.Wallets.Application.Configuration.Queries;
using App.Modules.Wallets.Application.Wallets.WalletsViewMetadata;
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

        const string sql = "SELECT d.id, d.user_id AS userId, d.title, d.currency, d.balance, d.created_at AS createdAt, d.is_removed AS isRemoved, " +
                           "v.wallet_id AS walletId, v.color, v.icon, v.is_considered_in_total_balance AS isConsideredInTotalBalance " +
                           "FROM wallets.debit_wallets d " +
                           "INNER JOIN wallets.wallet_view_metadata v ON d.id = v.wallet_id " +
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
