using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Incomes.GetIncome;
using App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.Domain.Incomes;

namespace App.Modules.FinanceTracking.IntegrationTests.Incomes;

[TestFixture]
public class EditIncomeTests : IncomesTestBase
{
    [Test]
    public async Task EditIncomeCommand_EditsIncome()
    {
        var userId = Guid.NewGuid();
        var incomeId = await AddNewIncomeAsync(userId);

        var newTargetWalletId = await CreateCashWallet(userId);
        var newCategoryId = await CreateCategory();

        var editCommand = await CreateEditIncomeCommand(incomeId, userId, newTargetWalletId, newCategoryId);

        await FinanceTrackingModule.ExecuteCommandAsync(editCommand);

        var income = await FinanceTrackingModule.ExecuteQueryAsync(new GetIncomeQuery(incomeId, userId));

        Assert.That(income, Is.Not.Null);
        Assert.That(income!.Id, Is.EqualTo(incomeId));
        Assert.That(income.TargetWalletId, Is.EqualTo(editCommand.TargetWalletId));
        Assert.That(income.CategoryId, Is.EqualTo(editCommand.CategoryId));
        Assert.That(income.Amount, Is.EqualTo(editCommand.Amount));
        Assert.That(income.Currency, Is.EqualTo(editCommand.Currency));
        Assert.That(income.MadeOn, Is.EqualTo(editCommand.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(income.Comment, Is.EqualTo(editCommand.Comment));
        Assert.That(income.Tags, Is.EquivalentTo(editCommand.Tags!));
    }

    [Test]
    public async Task EditIncomeCommand_UpdatesUserTags()
    {
        var userId = Guid.NewGuid();
        var incomeId = await AddNewIncomeAsync(userId: userId);

        var newTargetWalletId = await CreateCashWallet(userId);
        var newCategoryId = await CreateCategory();

        string[] newTags = ["New user tag 1", "New user tag 2"];
        var command = await CreateEditIncomeCommand(incomeId, userId, newTargetWalletId, newCategoryId, tags: newTags);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags!.Values, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task EditIncomeCommand_WithUnchangedTargetWallet_UpdatesBalanceOnCashWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateCashWallet(userId, initialBalance: 100);
        var incomeId = await AddNewIncomeAsync(userId, targetWalletId);

        var command = await CreateEditIncomeCommand(incomeId, userId, targetWalletId, amount: 300);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(400));
    }

    [Test]
    public async Task EditIncomeCommand_WithUnchangedTargetWallet_UpdatesBalanceOnDebitWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateDebitWallet(userId, initialBalance: 100);
        var incomeId = await AddNewIncomeAsync(userId, targetWalletId);

        var command = await CreateEditIncomeCommand(incomeId, userId, targetWalletId, amount: 300);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(400));
    }

    [Test]
    public async Task EditIncomeCommand_WithUnchangedTargetWallet_UpdatesBalanceOnCreditWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateCreditWallet(userId, initialAvailableBalance: 100);
        var incomeId = await AddNewIncomeAsync(userId, targetWalletId);

        var command = await CreateEditIncomeCommand(incomeId, userId, targetWalletId, amount: 300);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.AvailableBalance, Is.EqualTo(400));
    }

    [Test]
    public async Task EditIncomeCommand_WhenChangingTargetWallet_DecreasesBalanceOnOldWallet_And_IncreasesBalanceOnNewWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateCashWallet(userId, initialBalance: 100);
        var incomeId = await AddNewIncomeAsync(userId, targetWalletId);

        var newTargetWalletId = await CreateCashWallet(userId, initialBalance: 100);
        var command = await CreateEditIncomeCommand(incomeId, userId, newTargetWalletId, amount: 300);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var oldWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));
        var newWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(newTargetWalletId, userId));
        Assert.That(oldWallet!.Balance, Is.EqualTo(100));
        Assert.That(newWallet!.Balance, Is.EqualTo(400));
    }

    [Test]
    public async Task EditIncomeCommand_WhenIncomeDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingIncomeId = Guid.NewGuid();

        var command = await CreateEditIncomeCommand(notExistingIncomeId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Income>>());
    }

    [Test]
    public async Task EditIncomeCommand_WhenIncomeForUserIdDoesNotExist_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var incomeId = await AddNewIncomeAsync(userId: userId);

        var command = await CreateEditIncomeCommand(incomeId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<AccessDeniedException>());
    }

    [Test]
    public async Task EditIncomeCommand_WhenIncomeIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var emptyIncomeId = Guid.Empty;

        var command = await CreateEditIncomeCommand(emptyIncomeId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditIncomeCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var incomeId = Guid.NewGuid();

        var command = await CreateEditIncomeCommand(incomeId, userId: OptionalParameter.Default);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditIncomeCommand_WhenTargetWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var incomeId = await AddNewIncomeAsync();

        var command = await CreateEditIncomeCommand(incomeId, targetWalletId: Guid.Empty);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditIncomeCommand_WhenCategoryWithGivenIdDoesNotExist_ThrowsInvalidCommandException()
    {
        var incomeId = await AddNewIncomeAsync();

        var command = await CreateEditIncomeCommand(incomeId, categoryId: Guid.NewGuid());

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public async Task EditIncomeCommand_WhenAmountIsLessOrEqualToZero_ThrowsInvalidCommandException(int amount)
    {
        var incomeId = await AddNewIncomeAsync();

        var command = await CreateEditIncomeCommand(incomeId, amount: amount);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(null!)]
    [TestCase("")]
    [TestCase("PL")]
    [TestCase("PLNN")]
    public async Task EditIncomeCommand_WhenCurrencyIsNotIsoFormat_ThrowsInvalidCommandException(string currency)
    {
        var incomeId = await AddNewIncomeAsync();

        var command = await CreateEditIncomeCommand(incomeId, currency: currency);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditIncomeCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var incomeId = await AddNewIncomeAsync();

        var command = await CreateEditIncomeCommand(incomeId, madeOn: OptionalParameter.Default);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }
}
