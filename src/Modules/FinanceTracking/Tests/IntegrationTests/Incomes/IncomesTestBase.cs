using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Incomes.AddNewIncome;
using App.Modules.FinanceTracking.Application.Incomes.EditIncome;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.Incomes;

public class IncomesTestBase : TestBase
{
    protected async Task<AddNewIncomeCommand> CreateAddNewIncomeCommand(
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> targetWalletId = default,
        OptionalParameter<Guid> categoryId = default,
        OptionalParameter<int> amount = default,
        OptionalParameter<DateTime> madeOn = default,
        OptionalParameter<string> comment = default,
        OptionalParameter<IEnumerable<string>> tags = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());

        return new AddNewIncomeCommand(
            userIdValue,
            targetWalletId.GetValueOr(await CreateCashWallet(userIdValue)),
            categoryId.GetValueOr(await CreateCategory()),
            amount.GetValueOr(100),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Salary"),
            tags.GetValueOr(["Salary", "Savify"]));
    }

    protected async Task<Guid> AddNewIncomeAsync(OptionalParameter<Guid> userId = default, OptionalParameter<Guid> targetWalletId = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());
        var targetWalletIdValue = targetWalletId.GetValueOr(await CreateCashWallet(userIdValue));
        var categoryId = await CreateCategory();

        var command = new AddNewIncomeCommand(
            userId: userIdValue,
            targetWalletId: targetWalletIdValue,
            categoryId: categoryId,
            amount: 100,
            madeOn: DateTime.UtcNow,
            comment: "Salary",
            tags: ["Salary", "Savify"]);

        var incomeId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return incomeId;
    }

    protected async Task<EditIncomeCommand> CreateEditIncomeCommand(
        Guid incomeId,
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> targetWalletId = default,
        OptionalParameter<Guid> categoryId = default,
        OptionalParameter<int> amount = default,
        OptionalParameter<DateTime> madeOn = default,
        OptionalParameter<string> comment = default,
        OptionalParameter<IEnumerable<string>> tags = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());

        return new EditIncomeCommand(
            incomeId,
            userIdValue,
            targetWalletId.GetValueOr(await CreateCashWallet(userIdValue)),
            categoryId.GetValueOr(await CreateCategory()),
            amount.GetValueOr(500),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Edited income"),
            tags.GetValueOr(["Edited"]));
    }

    protected async Task<Guid> CreateCashWallet(Guid userId, int initialBalance = 100, string currency = "USD")
    {
        return await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId.Equals(Guid.Empty) ? Guid.NewGuid() : userId,
            "Cash wallet",
            currency,
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
