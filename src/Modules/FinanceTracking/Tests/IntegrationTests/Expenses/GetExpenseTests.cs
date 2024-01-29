using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;
using App.Modules.FinanceTracking.Application.Expenses.GetExpense;

namespace App.Modules.FinanceTracking.IntegrationTests.Expenses;

[TestFixture]
public class GetExpenseTests : ExpensesTestBase
{
    [Test]
    public async Task GetExpenseQuery_WhenExpenseDoesNotExist_ReturnsNull()
    {
        var expense = await FinanceTrackingModule.ExecuteQueryAsync(new GetExpenseQuery(Guid.NewGuid(), Guid.NewGuid()));

        Assert.That(expense, Is.Null);
    }

    [Test]
    public async Task GetExpenseQuery_WhenExpenseDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var expenseId = await AddNewExpenseAsync(userId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteQueryAsync(
            new GetExpenseQuery(expenseId, Guid.NewGuid())),
            Throws.TypeOf<AccessDeniedException>());
    }

    private async Task<Guid> AddNewExpenseAsync(OptionalParameter<Guid> userId = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());
        var sourceWalletId = await CreateCashWallet(userIdValue);
        var categoryId = await CreateCategory();

        var command = new AddNewExpenseCommand(
            userId: userIdValue,
            sourceWalletId: sourceWalletId,
            categoryId: categoryId,
            amount: 100,
            madeOn: DateTime.UtcNow,
            comment: "Clothes",
            tags: ["Clothes", "H&M"]);

        var expenseId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return expenseId;
    }
}
