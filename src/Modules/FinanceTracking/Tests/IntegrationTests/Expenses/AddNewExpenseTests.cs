using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;
using App.Modules.FinanceTracking.Application.Expenses.GetExpense;
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

namespace App.Modules.FinanceTracking.IntegrationTests.Expenses;

[TestFixture]
public class AddNewExpenseTests : TestBase
{
    [Test]
    public async Task AddNewExpenseCommand_AddsExpense()
    {
        var userId = Guid.NewGuid();
        var command = await CreateCommand(userId);

        var expenseId = await FinanceTrackingModule.ExecuteCommandAsync(command);
        var expense = await FinanceTrackingModule.ExecuteQueryAsync(new GetExpenseQuery(expenseId, userId));

        Assert.That(expense, Is.Not.Null);
        Assert.That(expense!.SourceWalletId, Is.EqualTo(command.SourceWalletId));
        Assert.That(expense.CategoryId, Is.EqualTo(command.CategoryId));
        Assert.That(expense.Amount, Is.EqualTo(command.Amount));
        Assert.That(expense.Currency, Is.EqualTo(command.Currency));
        Assert.That(expense.MadeOn, Is.EqualTo(command.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(expense.Comment, Is.EqualTo(command.Comment));
        Assert.That(expense.Tags, Is.EquivalentTo(command.Tags!));
    }

    [Test]
    public async Task AddNewExpenseCommand_UpdatesUserTags()
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
    public async Task AddNewExpenseCommand_DecreasesBalanceOnCashWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateCashWallet(userId, initialBalance: 500);
        var command = await CreateCommand(userId, sourceWalletId, amount: 200);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(300));
    }

    [Test]
    public async Task AddNewExpenseCommand_DecreasesBalanceOnDebitWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateDebitWallet(userId, initialBalance: 500);
        var command = await CreateCommand(userId, sourceWalletId, amount: 200);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(300));
    }

    [Test]
    public async Task AddNewExpenseCommand_DecreasesBalanceOnCreditWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateCreditWallet(userId, initialAvailableBalance: 500);
        var command = await CreateCommand(userId, sourceWalletId, amount: 200);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.AvailableBalance, Is.EqualTo(300));
    }

    [Test]
    public async Task AddNewExpenseCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = await CreateCommand(userId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewExpenseCommand_WhenSourceWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = await CreateCommand(sourceWalletId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewExpenseCommand_WhenCategoryWithGivenIdDoesNotExist_ThrowsInvalidCommandException()
    {
        var command = await CreateCommand(categoryId: Guid.NewGuid());

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public async Task AddNewExpenseCommand_WhenAmountIsLessOrEqualToZero_ThrowsInvalidCommandException(int amount)
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
    public async Task AddNewExpenseCommand_WhenCurrencyIsNotIsoFormat_ThrowsInvalidCommandException(string currency)
    {
        var command = await CreateCommand(currency: currency);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewExpenseCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var command = await CreateCommand(madeOn: OptionalParameter.Default);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    private async Task<AddNewExpenseCommand> CreateCommand(
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
