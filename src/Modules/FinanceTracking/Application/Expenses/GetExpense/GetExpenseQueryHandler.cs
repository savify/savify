using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.FinanceTracking.Application.Expenses.GetExpense;

internal class GetExpenseQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetExpenseQuery, ExpenseDto?>
{
    public async Task<ExpenseDto?> Handle(GetExpenseQuery query, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT e.id, e.user_id as userId, e.source_wallet_id as sourceWalletId, e.category_id as categoryId, e.amount, e.currency, e.made_on as madeOn, e.comment, e.tags
                   FROM {DatabaseConfiguration.Schema.Name}.expenses e
                   WHERE e.id = @ExpenseId
                   """;

        var expense = await connection.QuerySingleOrDefaultAsync<ExpenseDto>(sql, new { query.ExpenseId });

        if (expense is not null && expense.UserId != query.UserId)
        {
            throw new AccessDeniedException();
        }

        return expense;
    }
}
