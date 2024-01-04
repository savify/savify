using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;
using App.Modules.FinanceTracking.Application.Expenses.GetExpense;
using App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Expenses;

[TestFixture]
public class AddNewExpenseTests : TestBase
{
    [Test]
    public async Task AddNewExpenseCommand_ExpenseIsAdded()
    {
        var userId = Guid.NewGuid();

        var sourceWalletId = await CreateWallet(userId);
        var categoryId = Guid.NewGuid();

        var command = CreateCommand(userId, sourceWalletId, categoryId);

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
    public async Task AddNewExpenseCommand_UserTagsAreUpdated()
    {
        var userId = Guid.NewGuid();
        string[] newTags = ["New user tag 1", "New user tag 2"];

        var sourceWalletId = await CreateWallet(userId);
        var targetWalletId = await CreateWallet(userId);

        var command = CreateCommand(userId, sourceWalletId, targetWalletId, tags: newTags);

        _ = await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags!.Values, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task AddNewExpenseCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = CreateCommand(userId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewExpenseCommand_WhenSourceWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = CreateCommand(sourceWalletId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewExpenseCommand_WhenCategoryIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = CreateCommand(categoryId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public async Task AddNewExpenseCommand_WhenAmountIsLessOrEqualToZero_ThrowsInvalidCommandException(int amount)
    {
        var command = CreateCommand(amount: amount);

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
        var command = CreateCommand(currency: currency);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewExpenseCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var command = CreateCommand(madeOn: OptionalParameter.Default);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    private AddNewExpenseCommand CreateCommand(
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> sourceWalletId = default,
        OptionalParameter<Guid> categoryId = default,
        OptionalParameter<int> amount = default,
        OptionalParameter<string> currency = default,
        OptionalParameter<DateTime> madeOn = default,
        OptionalParameter<string> comment = default,
        OptionalParameter<IEnumerable<string>> tags = default)
    {
        return new AddNewExpenseCommand(
            userId.GetValueOr(Guid.NewGuid()),
            sourceWalletId.GetValueOr(Guid.NewGuid()),
            categoryId.GetValueOr(Guid.NewGuid()),
            amount.GetValueOr(100),
            currency.GetValueOr("USD"),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Clothes"),
            tags.GetValueOr(["Clothes", "H&M"]));
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
}
