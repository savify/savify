using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Configuration.Queries;
using App.Modules.FinanceTracking.Application.Wallets.WalletsViewMetadata;
using App.Modules.FinanceTracking.Domain.Wallets;
using Dapper;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;

internal class GetCreditWalletQueryHandler(ISqlConnectionFactory connectionFactory, ICreditWalletReadRepository creditWalletReadRepository)
    : IQueryHandler<GetCreditWalletQuery, CreditWalletDto?>
{
    public async Task<CreditWalletDto?> Handle(GetCreditWalletQuery query, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateNewConnection();

        var sql = $"""
                            SELECT c.id, c.user_id as userId, c.title, c.credit_limit AS creditLimit, c.currency, c.is_removed AS isRemoved,
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

        var creditWallet = creditWallets.SingleOrDefault();

        if (creditWallet is not null && creditWallet.UserId != query.UserId)
        {
            throw new AccessDeniedException();
        }

        if (creditWallet is not null)
        {
            creditWallet.AvailableBalance = await creditWalletReadRepository.GetAvailableBalanceAsync(new WalletId(query.WalletId));
        }

        return creditWallet;
    }
}
