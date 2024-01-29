using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Expenses.GetExpense;
using App.Modules.FinanceTracking.Application.Users.Tags.GetUserTags;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.Domain.Expenses;

namespace App.Modules.FinanceTracking.IntegrationTests.Expenses;

[TestFixture]
public class EditExpenseTests : ExpensesTestBase
{
    [Test]
    public async Task EditExpenseCommand_EditsExpense()
    {
        var userId = Guid.NewGuid();
        var expenseId = await AddNewExpenseAsync(userId);

        var newSourceWalletId = await CreateCashWallet(userId, currency: "PLN");
        var newCategoryId = await CreateCategory();

        var editCommand = await CreateEditExpenseCommand(expenseId, userId, newSourceWalletId, newCategoryId);

        await FinanceTrackingModule.ExecuteCommandAsync(editCommand);

        var expense = await FinanceTrackingModule.ExecuteQueryAsync(new GetExpenseQuery(expenseId, userId));

        Assert.That(expense, Is.Not.Null);
        Assert.That(expense!.Id, Is.EqualTo(expenseId));
        Assert.That(expense.SourceWalletId, Is.EqualTo(editCommand.SourceWalletId));
        Assert.That(expense.CategoryId, Is.EqualTo(editCommand.CategoryId));
        Assert.That(expense.Amount, Is.EqualTo(editCommand.Amount));
        Assert.That(expense.Currency, Is.EqualTo("PLN"));
        Assert.That(expense.MadeOn, Is.EqualTo(editCommand.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(expense.Comment, Is.EqualTo(editCommand.Comment));
        Assert.That(expense.Tags, Is.EquivalentTo(editCommand.Tags!));
    }

    [Test]
    public async Task EditExpenseCommand_UpdatesUserTags()
    {
        var userId = Guid.NewGuid();
        var expenseId = await AddNewExpenseAsync(userId: userId);

        var newSourceWalletId = await CreateCashWallet(userId);
        var newCategoryId = await CreateCategory();

        string[] newTags = ["New user tag 1", "New user tag 2"];
        var command = await CreateEditExpenseCommand(expenseId, userId, newSourceWalletId, newCategoryId, tags: newTags);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags!.Values, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task EditExpenseCommand_WithUnchangedSourceWallet_UpdatesBalanceOnCashWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateCashWallet(userId, initialBalance: 400);
        var incomeId = await AddNewExpenseAsync(userId, sourceWalletId);

        var command = await CreateEditExpenseCommand(incomeId, userId, sourceWalletId, amount: 300);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(100));
    }

    [Test]
    public async Task EditExpenseCommand_WithUnchangedSourceWallet_UpdatesBalanceOnDebitWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateDebitWallet(userId, initialBalance: 400);
        var incomeId = await AddNewExpenseAsync(userId, sourceWalletId);

        var command = await CreateEditExpenseCommand(incomeId, userId, sourceWalletId, amount: 300);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(100));
    }

    [Test]
    public async Task EditExpenseCommand_WithUnchangedSourceWallet_UpdatesBalanceOnCreditWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateCreditWallet(userId, initialAvailableBalance: 400);
        var incomeId = await AddNewExpenseAsync(userId, sourceWalletId);

        var command = await CreateEditExpenseCommand(incomeId, userId, sourceWalletId, amount: 300);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.AvailableBalance, Is.EqualTo(100));
    }

    [Test]
    public async Task EditExpenseCommand_WhenChangingSourceWallet_IncreasesBalanceOnOldWallet_And_DecreasesBalanceOnNewWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateCashWallet(userId, initialBalance: 400);
        var expenseId = await AddNewExpenseAsync(userId, sourceWalletId);

        var newSourceWalletId = await CreateCashWallet(userId, initialBalance: 400);
        var command = await CreateEditExpenseCommand(expenseId, userId, newSourceWalletId, amount: 300);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var oldWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        var newWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(newSourceWalletId, userId));
        Assert.That(oldWallet!.Balance, Is.EqualTo(400));
        Assert.That(newWallet!.Balance, Is.EqualTo(100));
    }

    [Test]
    public async Task EditExpenseCommand_WhenExpenseDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingExpenseId = Guid.NewGuid();

        var command = await CreateEditExpenseCommand(notExistingExpenseId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Expense>>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenExpenseForUserIdDoesNotExist_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var expenseId = await AddNewExpenseAsync(userId: userId);

        var command = await CreateEditExpenseCommand(expenseId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<AccessDeniedException>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenExpenseIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var emptyExpenseId = Guid.Empty;

        var command = await CreateEditExpenseCommand(emptyExpenseId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var expenseId = Guid.NewGuid();

        var command = await CreateEditExpenseCommand(expenseId, userId: OptionalParameter.Default);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenSourceWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var expenseId = await AddNewExpenseAsync();

        var command = await CreateEditExpenseCommand(expenseId, sourceWalletId: Guid.Empty);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenCategoryWithGivenIdDoesNotExist_ThrowsInvalidCommandException()
    {
        var expenseId = await AddNewExpenseAsync();

        var command = await CreateEditExpenseCommand(expenseId, categoryId: Guid.NewGuid());

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

        var command = await CreateEditExpenseCommand(expenseId, amount: amount);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditExpenseCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var expenseId = await AddNewExpenseAsync();

        var command = await CreateEditExpenseCommand(expenseId, madeOn: OptionalParameter.Default);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }
}
