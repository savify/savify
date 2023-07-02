using App.BuildingBlocks.Application.Data;
using App.Modules.Accounts.Application.Accounts.AccountsViewMetadata;
using App.Modules.Accounts.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Accounts.Application.Accounts.DebitAccounts.GetDebitAccount;

internal class GetDebitAccountQueryHandler : IQueryHandler<GetDebitAccountQuery, DebitAccountDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetDebitAccountQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<DebitAccountDto> Handle(GetDebitAccountQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = "SELECT d.id, d.user_id AS userId, d.title, d.currency, d.balance, d.created_at AS createdAt, " +
                           "v.account_id AS accountId, v.color, v.icon, v.is_considered_in_total_balance AS isConsideredInTotalBalance " +
                           "FROM accounts.debit_accounts d " +
                           "INNER JOIN accounts.account_view_metadata v ON d.id = v.account_id " +
                           "WHERE d.id = @AccountId";

        var debitAccounts = await connection.QueryAsync<DebitAccountDto, AccountViewMetadataDto, DebitAccountDto>(sql, (debitAccount, viewMetadata) =>
        {
            debitAccount.ViewMetadata = viewMetadata;
            return debitAccount;
        },
        new { request.AccountId },
        splitOn: "AccountId");

        var debitAccount = debitAccounts.SingleOrDefault();
        return debitAccount;
    }
}
