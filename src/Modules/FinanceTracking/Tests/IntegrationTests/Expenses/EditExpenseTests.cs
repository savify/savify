using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;
using App.Modules.FinanceTracking.Application.Expenses.EditExpense;
using App.Modules.FinanceTracking.Application.Expenses.GetExpense;
using App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.Expenses;

[TestFixture]
public class EditExpenseTests : TestBase
{
    [Test]
    public async Task EditExpenseCommand_ExpenseIsEdited()
    {
        var userId = Guid.NewGuid();
        var expenseId = await AddNewExpenseAsync(userId);

        var newSourceWalletId = await CreateWallet(userId);
        var newCategoryId = await CreateCategory();

        var editCommand = await CreateCommand(expenseId, userId, newSourceWalletId, newCategoryId);

        await FinanceTrackingModule.ExecuteCommandAsync(editCommand);

        var expense = await FinanceTrackingModule.ExecuteQueryAsync(new GetExpenseQuery(expenseId, userId));

        Assert.That(expense, Is.Not.Null);
        Assert.That(expense!.Id, Is.EqualTo(expenseId));
        Assert.That(expense.SourceWalletId, Is.EqualTo(editCommand.SourceWalletId));
        Assert.That(expense.CategoryId, Is.EqualTo(editCommand.CategoryId));
        Assert.That(expense.Amount, Is.EqualTo(editCommand.Amount));
        Assert.That(expense.Currency, Is.EqualTo(editCommand.Currency));
        Assert.That(expense.MadeOn, Is.EqualTo(editCommand.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(expense.Comment, Is.EqualTo(editCommand.Comment));
        Assert.That(expense.Tags, Is.EquivalentTo(editCommand.Tags!));
    }

    [Test]
    public async Task EditExpenseCommand_UserTagsAreUpdated()
    {
        var userId = Guid.NewGuid();
        var expenseId = await AddNewExpenseAsync(userId: userId);

        var newSourceWalletId = await CreateWallet(userId);
        var newCategoryId = await CreateCategory();

        string[] newTags = ["New user tag 1", "New user tag 2"];
        var command = await CreateCommand(expenseId, userId, newSourceWalletId, newCategoryId, tags: newTags);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags!.Values, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task EditExpenseCommand_WhenExpenseDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingExpenseId = Guid.NewGuid();

        var command = await CreateCommand(notExistingExpenseId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Expense>>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenExpenseForUserIdDoesNotExist_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var expenseId = await AddNewExpenseAsync(userId: userId);

        var command = await CreateCommand(expenseId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<AccessDeniedException>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenExpenseIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var emptyExpenseId = Guid.Empty;

        var command = await CreateCommand(emptyExpenseId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var expenseId = Guid.NewGuid();

        var command = await CreateCommand(expenseId, userId: OptionalParameter.Default);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenSourceWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var expenseId = await AddNewExpenseAsync();

        var command = await CreateCommand(expenseId, sourceWalletId: Guid.Empty);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenCategoryWithGivenIdDoesNotExist_ThrowsInvalidCommandException()
    {
        var expenseId = await AddNewExpenseAsync();

        var command = await CreateCommand(expenseId, categoryId: Guid.NewGuid());

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public async Task EditExpenseCommand_WhenAmountIsLessOrEqualToZero_ThrowsInvalidCommandException(int amount)
    {
        var expenseId = await AddNewExpenseAsync();

        var command = await CreateCommand(expenseId, amount: amount);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(null!)]
    [TestCase("")]
    [TestCase("PL")]
    [TestCase("PLNN")]
    public async Task EditExpenseCommand_WhenCurrencyIsNotIsoFormat_ThrowsInvalidCommandException(string currency)
    {
        var expenseId = await AddNewExpenseAsync();

        var command = await CreateCommand(expenseId, currency: currency);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var expenseId = await AddNewExpenseAsync();

        var command = await CreateCommand(expenseId, madeOn: OptionalParameter.Default);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    private async Task<Guid> AddNewExpenseAsync(OptionalParameter<Guid> userId = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());
        var sourceWalletId = await CreateWallet(userIdValue);
        var categoryId = await CreateCategory();

        var command = new AddNewExpenseCommand(
            userId: userIdValue,
            sourceWalletId: sourceWalletId,
            categoryId: categoryId,
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Savings expense",
            tags: ["Savings", "Minor"]);

        var expenseId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return expenseId;
    }

    private async Task<EditExpenseCommand> CreateCommand(
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
            sourceWalletId.GetValueOr(await CreateWallet(userIdValue)),
            categoryId.GetValueOr(await CreateCategory()),
            amount.GetValueOr(500),
            currency.GetValueOr("PLN"),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Edited expense"),
            tags.GetValueOr(["Edited"]));
    }

    private async Task<Guid> CreateWallet(Guid userId)
    {
        return await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId.Equals(Guid.Empty) ? Guid.NewGuid() : userId,
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
