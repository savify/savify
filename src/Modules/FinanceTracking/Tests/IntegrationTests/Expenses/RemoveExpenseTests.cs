using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;
using App.Modules.FinanceTracking.Application.Expenses.GetExpense;
using App.Modules.FinanceTracking.Application.Expenses.RemoveExpense;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
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
    public async Task RemoveExpenseCommand_IncreasesBalanceOnCashWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateCashWallet(userId, initialBalance: 100);
        var incomeId = await AddNewExpenseAsync(userId, sourceWalletId);

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveExpenseCommand(incomeId, userId));

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(100));
    }

    [Test]
    public async Task RemoveExpenseCommand_IncreasesBalanceOnDebitWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateDebitWallet(userId, initialBalance: 100);
        var incomeId = await AddNewExpenseAsync(userId, sourceWalletId);

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveExpenseCommand(incomeId, userId));

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(100));
    }

    [Test]
    public async Task RemoveExpenseCommand_IncreasesBalanceOnCreditWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateCreditWallet(userId, initialAvailableBalance: 100);
        var incomeId = await AddNewExpenseAsync(userId, sourceWalletId);

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveExpenseCommand(incomeId, userId));

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.AvailableBalance, Is.EqualTo(100));
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

    private async Task<Guid> AddNewExpenseAsync(Guid userId, Guid? sourceWalletId = null)
    {
        var categoryId = await CreateCategory();

        var command = new AddNewExpenseCommand(
            userId: userId,
            sourceWalletId: sourceWalletId ?? await CreateCashWallet(userId),
            categoryId: categoryId,
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Clothes",
            tags: ["Clothes", "H&M"]);

        var expenseId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return expenseId;
    }

    private async Task<Guid> CreateCashWallet(Guid userId, int initialBalance = 100)
    {
        return await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId.Equals(Guid.Empty) ? Guid.NewGuid() : userId,
            "Cash wallet",
            "USD",
            initialBalance,
            "#000000",
            "https://cdn.savify.io/icons/icon.svg",
            true));
    }

    private async Task<Guid> CreateDebitWallet(Guid userId, int initialBalance = 100)
    {
        return await FinanceTrackingModule.ExecuteCommandAsync(new AddNewDebitWalletCommand(
            userId.Equals(Guid.Empty) ? Guid.NewGuid() : userId,
            "Debit wallet",
            "USD",
            initialBalance,
            "#000000",
            "https://cdn.savify.io/icons/icon.svg",
            true));
    }

    private async Task<Guid> CreateCreditWallet(Guid userId, int initialAvailableBalance = 100)
    {
        return await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCreditWalletCommand(
            userId.Equals(Guid.Empty) ? Guid.NewGuid() : userId,
            "Debit wallet",
            "USD",
            initialAvailableBalance,
            2000,
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
