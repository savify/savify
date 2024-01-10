using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;
using App.Modules.FinanceTracking.Application.Expenses.EditExpense;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.Expenses;

public class ExpensesTestBase : TestBase
{
    protected async Task<AddNewExpenseCommand> CreateAddNewExpenseCommand(
    OptionalParameter<Guid> userId = default,
    OptionalParameter<Guid> sourceWalletId = default,
    OptionalParameter<Guid> categoryId = default,
    OptionalParameter<int> amount = default,
    OptionalParameter<string> currency = default,
    OptionalParameter<DateTime> madeOn = default,
    OptionalParameter<string> comment = default,
    OptionalParameter<IEnumerable<string>> tags = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());

        return new AddNewExpenseCommand(
            userIdValue,
            sourceWalletId.GetValueOr(await CreateCashWallet(userIdValue)),
            categoryId.GetValueOr(await CreateCategory()),
            amount.GetValueOr(100),
            currency.GetValueOr("USD"),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Clothes"),
            tags.GetValueOr(["Clothes", "H&M"]));
    }

    protected async Task<Guid> AddNewExpenseAsync(OptionalParameter<Guid> userId = default, OptionalParameter<Guid> sourceWalletId = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());
        var sourceWalletIdValue = sourceWalletId.GetValueOr(await CreateCashWallet(userIdValue));
        var categoryId = await CreateCategory();

        var command = new AddNewExpenseCommand(
            userId: userIdValue,
            sourceWalletId: sourceWalletIdValue,
            categoryId: categoryId,
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Savings expense",
            tags: ["Savings", "Minor"]);

        var expenseId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return expenseId;
    }

    protected async Task<EditExpenseCommand> CreateEditExpenseCommand(
        Guid expenseId,
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> sourceWalletId = default,
        OptionalParameter<Guid> categoryId = default,
        OptionalParameter<int> amount = default,
        OptionalParameter<string> currency = default,
        OptionalParameter<DateTime> madeOn = default,
        OptionalParameter<string> comment = default,
        OptionalParameter<IEnumerable<string>> tags = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());

        return new EditExpenseCommand(
            expenseId,
            userIdValue,
            sourceWalletId.GetValueOr(await CreateCashWallet(userIdValue)),
            categoryId.GetValueOr(await CreateCategory()),
            amount.GetValueOr(500),
            currency.GetValueOr("PLN"),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Edited expense"),
            tags.GetValueOr(["Edited"]));
    }

    protected async Task<Guid> CreateCashWallet(Guid userId, int initialBalance = 100)
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

    protected async Task<Guid> CreateDebitWallet(Guid userId, int initialBalance = 100)
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

    protected async Task<Guid> CreateCreditWallet(Guid userId, int initialAvailableBalance = 100)
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

    protected async Task<Guid> CreateCategory()
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var categoryId = Guid.NewGuid();

        var sql = $"INSERT INTO {DatabaseConfiguration.Schema.Name}.categories (id, external_id) VALUES (@Id, @ExternalId)";

        await sqlConnection.ExecuteAsync(sql, new { Id = categoryId, ExternalId = Guid.NewGuid() });

        return categoryId;
    }
}
