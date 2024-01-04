using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;
using App.Modules.FinanceTracking.Application.Expenses.GetExpense;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Expenses;

[TestFixture]
public class GetExpenseTests : TestBase
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
        var sourceWalletId = await CreateWallet(userIdValue);

        var command = new AddNewExpenseCommand(
            userId: userIdValue,
            sourceWalletId: sourceWalletId,
            categoryId: Guid.NewGuid(),
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Clothes",
            tags: ["Clothes", "H&M"]);

        var expenseId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return expenseId;
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
