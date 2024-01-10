using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Expenses.GetExpense;
using App.Modules.FinanceTracking.Application.Expenses.RemoveExpense;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.Domain.Expenses;

namespace App.Modules.FinanceTracking.IntegrationTests.Expenses;

[TestFixture]
public class RemoveExpenseTests : ExpensesTestBase
{
    [Test]
    public async Task RemoveExpenseCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var expenseId = await AddNewExpenseAsync(userId);

        var command = new RemoveExpenseCommand(expenseId, userId);
        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var expense = await FinanceTrackingModule.ExecuteQueryAsync(new GetExpenseQuery(expenseId, userId));

        Assert.That(expense, Is.Null);
    }

    [Test]
    public async Task RemoveExpenseCommand_IncreasesBalanceOnCashWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateCashWallet(userId, initialBalance: 100);
        var incomeId = await AddNewExpenseAsync(userId, sourceWalletId);

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveExpenseCommand(incomeId, userId));

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(100));
    }

    [Test]
    public async Task RemoveExpenseCommand_IncreasesBalanceOnDebitWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateDebitWallet(userId, initialBalance: 100);
        var incomeId = await AddNewExpenseAsync(userId, sourceWalletId);

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveExpenseCommand(incomeId, userId));

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(100));
    }

    [Test]
    public async Task RemoveExpenseCommand_IncreasesBalanceOnCreditWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateCreditWallet(userId, initialAvailableBalance: 100);
        var incomeId = await AddNewExpenseAsync(userId, sourceWalletId);

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveExpenseCommand(incomeId, userId));

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(sourceWalletId, userId));
        Assert.That(wallet!.AvailableBalance, Is.EqualTo(100));
    }

    [Test]
    public async Task RemoveExpenseCommand_WhenExpenseIdIsDefaultGuid_ThrowsInvalidCommandException()
    {
        var command = new RemoveExpenseCommand(Guid.Empty, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task RemoveExpenseCommand_WhenExpenseDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingExpenseId = Guid.NewGuid();
        var command = new RemoveExpenseCommand(notExistingExpenseId, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Expense>>());
    }

    [Test]
    public async Task RemoveExpenseCommand_WhenExpenseForUserIdDoesNotExist_ThrowsAccessDeniedException()
    {
        var expenseId = await AddNewExpenseAsync(Guid.NewGuid());
        var command = new RemoveExpenseCommand(expenseId, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<AccessDeniedException>());
    }
}
