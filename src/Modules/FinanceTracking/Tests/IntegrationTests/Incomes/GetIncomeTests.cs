using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Incomes.AddNewIncome;
using App.Modules.FinanceTracking.Application.Incomes.GetIncome;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.Incomes;

[TestFixture]
public class GetIncomeTests : TestBase
{
    [Test]
    public async Task GetIncomeQuery_WhenIncomeDoesNotExist_ReturnsNull()
    {
        var income = await FinanceTrackingModule.ExecuteQueryAsync(new GetIncomeQuery(Guid.NewGuid(), Guid.NewGuid()));

        Assert.That(income, Is.Null);
    }

    [Test]
    public async Task GetIncomeQuery_WhenIncomeDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var incomeId = await AddNewIncomeAsync(userId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteQueryAsync(
            new GetIncomeQuery(incomeId, Guid.NewGuid())),
            Throws.TypeOf<AccessDeniedException>());
    }

    private async Task<Guid> AddNewIncomeAsync(OptionalParameter<Guid> userId = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());
        var targetWalletId = await CreateWallet(userIdValue);
        var categoryId = await CreateCategory();

        var command = new AddNewIncomeCommand(
            userId: userIdValue,
            targetWalletId: targetWalletId,
            categoryId: categoryId,
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Salary",
            tags: ["Salary", "Savify"]);

        var incomeId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return incomeId;
    }

    private async Task<Guid> CreateWallet(Guid userId)
    {
        return await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId,
            "Cash wallet",
            "USD",
            100,
            "#000000",
            "https://cdn.savify.io/icons/icon.svg",
            true));
    }

    private async Task<Guid> CreateCategory()
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var categoryId = Guid.NewGuid();

        var sql = $"INSERT INTO {DatabaseConfiguration.Schema.Name}.categories (id, external_id) VALUES (@Id, @ExternalId)";

        await sqlConnection.ExecuteAsync(sql, new { Id = categoryId, ExternalId = Guid.NewGuid() });

        return categoryId;
    }
}
