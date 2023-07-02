using App.BuildingBlocks.Application.Data;
using App.Modules.Accounts.Application.AccountsViewMetadata;
using App.Modules.Accounts.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Accounts.Application.CreditAccounts.GetCreditAccount;

public class GetCreditAccountQueryHandler : IQueryHandler<GetCreditAccountQuery, CreditAccountDto>
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetCreditAccountQueryHandler(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<CreditAccountDto> Handle(GetCreditAccountQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateNewConnection();

        const string sql = "SELECT c.id, c.title, c.available_balance AS availableBalance, c.credit_limit AS creditLimit, c.created_at AS createdAt, " +
                           "v.account_id as accountId, v.color, v.icon, v.is_considered_in_total_balance AS isConsideredInTotalBalance" +
                           "FROM cretid_accounts c " +
                           "INNER JOIN account_view_metadata v ON c.Id = v.AccountId" +
                           "WHERE c.id = @AccountId";

        var creditAccounts = await connection.QueryAsync<CreditAccountDto, AccountViewMetadataDto, CreditAccountDto>(sql, (creditAccount, viewMetadata) =>
        {
            creditAccount.ViewMetadata = viewMetadata;
            return creditAccount;
        },
        new { request.AccountId },
        splitOn: "accountId");

        var creditAccount = creditAccounts.SingleOrDefault();
        return creditAccount;
    }
}
