using App.BuildingBlocks.Application.Data;
using App.Modules.Wallets.Application.Configuration.Data;
using App.Modules.Wallets.Application.Configuration.Queries;
using App.Modules.Wallets.Application.Wallets.WalletsViewMetadata;
using Dapper;

namespace App.Modules.Wallets.Application.Wallets.CreditWallets.GetCreditWallet;

internal class GetCreditWalletQueryHandler : IQueryHandler<GetCreditWalletQuery, CreditWalletDto?>
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetCreditWalletQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<CreditWalletDto?> Handle(GetCreditWalletQuery query, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        var sql = $"""
                            SELECT c.id, c.user_id as userId, c.title, c.available_balance AS availableBalance,
                                   c.credit_limit AS creditLimit, c.currency, c.created_at AS createdAt, c.is_removed AS isRemoved,
                                   v.wallet_id as walletId, v.color, v.icon, v.is_considered_in_total_balance AS isConsideredInTotalBalance
                            FROM {DatabaseConfiguration.Schema.Name}.credit_wallets c
                                INNER JOIN {DatabaseConfiguration.Schema.Name}.wallet_view_metadata v ON c.id = v.wallet_id
                            WHERE c.id = @WalletId
                            """;

        var creditWallets = await connection.QueryAsync<CreditWalletDto, WalletViewMetadataDto, CreditWalletDto>(sql, (creditWallet, viewMetadata) =>
        {
            creditWallet.ViewMetadata = viewMetadata;

            return creditWallet;
        },
        new { query.WalletId },
        splitOn: "walletId");

        return creditWallets.SingleOrDefault(); ;
    }
}
