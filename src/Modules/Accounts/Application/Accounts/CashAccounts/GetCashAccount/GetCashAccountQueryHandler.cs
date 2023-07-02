using App.BuildingBlocks.Application.Data;
using App.Modules.Accounts.Application.Accounts.AccountsViewMetadata;
using App.Modules.Accounts.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Accounts.Application.Accounts.CashAccounts.GetCashAccount;
internal class GetCashAccountQueryHandler : IQueryHandler<GetCashAccountQuery, CashAccountDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCashAccountQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<CashAccountDto> Handle(GetCashAccountQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = "SELECT c.id, c.user_id AS userId, c.title, c.currency, c.balance, c.created_at AS createdAt, " +
                           "v.account_id AS accountId, v.color, v.icon, v.is_considered_in_total_balance AS isConsideredInTotalBalance " +
                           "FROM accounts.cash_accounts c " +
                           "INNER JOIN accounts.account_view_metadata v ON c.id = v.account_id " +
                           "WHERE id = @AccountId";

        var cashAccounts = await connection.QueryAsync<CashAccountDto, AccountViewMetadataDto, CashAccountDto>(sql, (cashAccount, viewMetadata) =>
        {
            cashAccount.ViewMetadata = viewMetadata;
            return cashAccount;
        },
        new { request.AccountId },
        splitOn: "accountId");

        var cashAccount = cashAccounts.SingleOrDefault();
        return cashAccount;
    }
}
