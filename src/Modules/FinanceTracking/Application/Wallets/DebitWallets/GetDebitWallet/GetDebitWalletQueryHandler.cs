using System.Data;
using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Configuration.Queries;
using App.Modules.FinanceTracking.Application.Wallets.WalletsViewMetadata;
using App.Modules.FinanceTracking.Domain.Wallets;
using Dapper;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;

internal class GetDebitWalletQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IDebitWalletReadRepository debitWalletReadRepository)
    : IQueryHandler<GetDebitWalletQuery, DebitWalletDto?>
{
    public async Task<DebitWalletDto?> Handle(GetDebitWalletQuery query, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                           SELECT d.id, d.user_id AS userId, d.title, d.currency, d.is_removed AS isRemoved,
                               d.bank_connection_id AS bankConnectionId, d.bank_account_id AS bankAccountId,
                               v.wallet_id AS walletId, v.color, v.icon, v.is_considered_in_total_balance AS isConsideredInTotalBalance
                           FROM {DatabaseConfiguration.Schema.Name}.debit_wallets d
                           INNER JOIN {DatabaseConfiguration.Schema.Name}.wallet_view_metadata v ON d.id = v.wallet_id
                           WHERE d.id = @WalletId
                           """;

        var debitWallets = await connection.QueryAsync<DebitWalletDto, WalletViewMetadataDto, DebitWalletDto>(sql, (debitWallet, viewMetadata) =>
        {
            debitWallet.ViewMetadata = viewMetadata;

            return debitWallet;
        },
        new { query.WalletId },
        splitOn: "walletId");

        var debitWallet = debitWallets.SingleOrDefault();

        if (debitWallet is not null && debitWallet.UserId != query.UserId)
        {
            throw new AccessDeniedException();
        }

        if (debitWallet is not null)
        {
            debitWallet.Balance = await debitWalletReadRepository.GetBalanceAsync(new WalletId(query.WalletId));
            debitWallet.ManualBalanceChanges = await GetManualBalanceChanges(query.WalletId, connection);
        }

        return debitWallet;
    }

    private async Task<IEnumerable<ManualBalanceChangeDto>> GetManualBalanceChanges(Guid walletId, IDbConnection connection)
    {
        var manualChangesSql = $"""
                                SELECT type, amount, currency, made_on AS madeOn
                                FROM {DatabaseConfiguration.Schema.Name}.debit_wallet_manual_balance_changes WHERE wallet_id = @WalletId
                                """;

        return await connection.QueryAsync<ManualBalanceChangeDto>(manualChangesSql, new { WalletId = walletId });
    }
}
