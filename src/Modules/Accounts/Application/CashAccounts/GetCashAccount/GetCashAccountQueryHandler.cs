using App.BuildingBlocks.Application.Data;
using App.Modules.Accounts.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Accounts.Application.CashAccounts.GetCashAccount;
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

        string sql = "SELECT id, user_id As userId, title, currency, balance, created_at AS createdAt " +
                     "FROM accounts.cash_accounts " +
                     "WHERE id = @AccountId";

        return await connection.QuerySingleOrDefaultAsync<CashAccountDto>(sql, new { request.AccountId });
    }
}
