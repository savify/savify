using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Incomes.GetIncome;
using App.Modules.FinanceTracking.Application.Incomes.RemoveIncome;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.Domain.Incomes;

namespace App.Modules.FinanceTracking.IntegrationTests.Incomes;

[TestFixture]
public class RemoveIncomeTests : IncomesTestBase
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
}
