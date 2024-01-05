using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;
using App.Modules.FinanceTracking.Application.Expenses.GetExpense;
using App.Modules.FinanceTracking.Application.Expenses.RemoveExpense;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.Expenses;

[TestFixture]
public class RemoveExpenseTests : TestBase
{
    [Test]
    public async Task RemoveExpenseCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var expenseId = await AddNewExpenseAsync(userId);

        var command = new RemoveExpenseCommand(expenseId, userId);
        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var expense = await FinanceTrackingModule.ExecuteQueryAsync(new GetExpenseQuery(expenseId, userId));

        Assert.That(expense, Is.Null);
    }

    [Test]
    public async Task RemoveExpenseCommand_WhenExpenseIdIsDefaultGuid_ThrowsInvalidCommandException()
    {
        var command = new RemoveExpenseCommand(Guid.Empty, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task RemoveExpenseCommand_WhenExpenseDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingExpenseId = Guid.NewGuid();
        var command = new RemoveExpenseCommand(notExistingExpenseId, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Expense>>());
    }

    [Test]
    public async Task RemoveExpenseCommand_WhenExpenseForUserIdDoesNotExist_ThrowsAccessDeniedException()
    {
        var expenseId = await AddNewExpenseAsync(Guid.NewGuid());
        var command = new RemoveExpenseCommand(expenseId, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<AccessDeniedException>());
    }

    private async Task<Guid> AddNewExpenseAsync(Guid userId)
    {
        var sourceWalletId = await CreateWallet(userId);
        var categoryId = await CreateCategory();

        var command = new AddNewExpenseCommand(
            userId: userId,
            sourceWalletId: sourceWalletId,
            categoryId: categoryId,
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Clothes",
            tags: ["Clothes", "H&M"]);

        var expenseId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return expenseId;
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
