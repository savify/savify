using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Incomes.AddNewIncome;
using App.Modules.FinanceTracking.Application.Incomes.GetIncome;
using App.Modules.FinanceTracking.Application.Incomes.RemoveIncome;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.Domain.Incomes;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.Incomes;

[TestFixture]
public class RemoveIncomeTests : TestBase
{
    [Test]
    public async Task RemoveIncomeCommand_RemovesIncome()
    {
        var userId = Guid.NewGuid();
        var incomeId = await AddNewIncomeAsync(userId);

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveIncomeCommand(incomeId, userId));

        var income = await FinanceTrackingModule.ExecuteQueryAsync(new GetIncomeQuery(incomeId, userId));

        Assert.That(income, Is.Null);
    }

    [Test]
    public async Task RemoveIncomeCommand_DecreasesBalanceOnCashWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateCashWallet(userId, initialBalance: 100);
        var incomeId = await AddNewIncomeAsync(userId, targetWalletId);

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveIncomeCommand(incomeId, userId));

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(100));
    }

    [Test]
    public async Task RemoveIncomeCommand_DecreasesBalanceOnDebitWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateDebitWallet(userId, initialBalance: 100);
        var incomeId = await AddNewIncomeAsync(userId, targetWalletId);

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveIncomeCommand(incomeId, userId));

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(100));
    }

    [Test]
    public async Task RemoveIncomeCommand_DecreasesBalanceOnCreditWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateCreditWallet(userId, initialAvailableBalance: 100);
        var incomeId = await AddNewIncomeAsync(userId, targetWalletId);

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveIncomeCommand(incomeId, userId));

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.AvailableBalance, Is.EqualTo(100));
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

    private async Task<Guid> AddNewIncomeAsync(Guid userId, Guid? targetWalletId = null)
    {
        var categoryId = await CreateCategory();

        var command = new AddNewIncomeCommand(
            userId: userId,
            targetWalletId: targetWalletId ?? await CreateCashWallet(userId),
            categoryId: categoryId,
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Salary",
            tags: ["Salary", "Savify"]);

        var incomeId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return incomeId;
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
