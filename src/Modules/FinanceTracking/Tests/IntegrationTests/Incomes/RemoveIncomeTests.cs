using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Incomes.AddNewIncome;
using App.Modules.FinanceTracking.Application.Incomes.GetIncome;
using App.Modules.FinanceTracking.Application.Incomes.RemoveIncome;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Domain.Incomes;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.Incomes;

[TestFixture]
public class RemoveIncomeTests : TestBase
{
    [Test]
    public async Task RemoveIncomeCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var incomeId = await AddNewIncomeAsync(userId);

        var command = new RemoveIncomeCommand(incomeId, userId);
        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var income = await FinanceTrackingModule.ExecuteQueryAsync(new GetIncomeQuery(incomeId, userId));

        Assert.That(income, Is.Null);
    }

    [Test]
    public async Task RemoveIncomeCommand_WhenIncomeIdIsDefaultGuid_ThrowsInvalidCommandException()
    {
        var command = new RemoveIncomeCommand(Guid.Empty, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task RemoveIncomeCommand_WhenIncomeDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingIncomeId = Guid.NewGuid();
        var command = new RemoveIncomeCommand(notExistingIncomeId, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Income>>());
    }

    [Test]
    public async Task RemoveIncomeCommand_WhenIncomeForUserIdDoesNotExist_ThrowsAccessDeniedException()
    {
        var incomeId = await AddNewIncomeAsync(Guid.NewGuid());
        var command = new RemoveIncomeCommand(incomeId, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<AccessDeniedException>());
    }

    private async Task<Guid> AddNewIncomeAsync(Guid userId)
    {
        var targetWalletId = await CreateWallet(userId);
        var categoryId = await CreateCategory();

        var command = new AddNewIncomeCommand(
            userId: userId,
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
