using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.FinanceTracking.Application.Incomes.GetIncome;

internal class GetIncomeQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetIncomeQuery, IncomeDto?>
{
    public async Task<IncomeDto?> Handle(GetIncomeQuery query, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT e.id, e.user_id as userId, e.target_wallet_id as targetWalletId, e.category_id as categoryId, e.amount, e.currency, e.made_on as madeOn, e.comment, e.tags
                   FROM {DatabaseConfiguration.Schema.Name}.incomes e
                   WHERE e.id = @IncomeId
                   """;

        var income = await connection.QuerySingleOrDefaultAsync<IncomeDto>(sql, new { query.IncomeId });

        if (income is not null && income.UserId != query.UserId)
        {
            throw new AccessDeniedException();
        }

        return income;
    }
}
