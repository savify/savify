using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Incomes.AddNewIncome;
using App.Modules.FinanceTracking.Application.Incomes.GetIncome;
using App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.Incomes;

[TestFixture]
public class AddNewIncomeTests : TestBase
{
    [Test]
    public async Task AddNewIncomeCommand_AddsIncome()
    {
        var userId = Guid.NewGuid();
        var command = await CreateCommand(userId);

        var incomeId = await FinanceTrackingModule.ExecuteCommandAsync(command);
        var income = await FinanceTrackingModule.ExecuteQueryAsync(new GetIncomeQuery(incomeId, userId));

        Assert.That(income, Is.Not.Null);
        Assert.That(income!.TargetWalletId, Is.EqualTo(command.TargetWalletId));
        Assert.That(income.CategoryId, Is.EqualTo(command.CategoryId));
        Assert.That(income.Amount, Is.EqualTo(command.Amount));
        Assert.That(income.Currency, Is.EqualTo(command.Currency));
        Assert.That(income.MadeOn, Is.EqualTo(command.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(income.Comment, Is.EqualTo(command.Comment));
        Assert.That(income.Tags, Is.EquivalentTo(command.Tags!));
    }

    [Test]
    public async Task AddNewIncomeCommand_IncreasesBalanceOnCashWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateCashWallet(userId, initialBalance: 100);
        var command = await CreateCommand(userId, targetWalletId, amount: 200);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(300));
    }

    [Test]
    public async Task AddNewIncomeCommand_IncreasesBalanceOnDebitWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateDebitWallet(userId, initialBalance: 100);
        var command = await CreateCommand(userId, targetWalletId, amount: 200);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(300));
    }

    [Test]
    public async Task AddNewIncomeCommand_IncreasesBalanceOnCreditWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateCreditWallet(userId, initialAvailableBalance: 100);
        var command = await CreateCommand(userId, targetWalletId, amount: 200);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.AvailableBalance, Is.EqualTo(300));
    }

    [Test]
    public async Task AddNewIncomeCommand_UpdatesUserTags()
    {
        var userId = Guid.NewGuid();
        string[] newTags = ["New user tag 1", "New user tag 2"];

        var command = await CreateCommand(userId, tags: newTags);

        _ = await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags!.Values, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task AddNewIncomeCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = await CreateCommand(userId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewIncomeCommand_WhenTargetWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = await CreateCommand(targetWalletId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewIncomeCommand_WhenCategoryWithGivenIdDoesNotExist_ThrowsInvalidCommandException()
    {
        var command = await CreateCommand(categoryId: Guid.NewGuid());

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public async Task AddNewIncomeCommand_WhenAmountIsLessOrEqualToZero_ThrowsInvalidCommandException(int amount)
    {
        var command = await CreateCommand(amount: amount);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(null!)]
    [TestCase("")]
    [TestCase("PL")]
    [TestCase("PLNN")]
    public async Task AddNewIncomeCommand_WhenCurrencyIsNotIsoFormat_ThrowsInvalidCommandException(string currency)
    {
        var command = await CreateCommand(currency: currency);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewIncomeCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var command = await CreateCommand(madeOn: OptionalParameter.Default);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    private async Task<AddNewIncomeCommand> CreateCommand(
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> targetWalletId = default,
        OptionalParameter<Guid> categoryId = default,
        OptionalParameter<int> amount = default,
        OptionalParameter<string> currency = default,
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
            currency.GetValueOr("USD"),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Salary"),
            tags.GetValueOr(["Salary", "Savify"]));
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
