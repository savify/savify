using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Incomes.GetIncome;
using App.Modules.FinanceTracking.Application.Users.Tags.GetUserTags;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;

namespace App.Modules.FinanceTracking.IntegrationTests.Incomes;

[TestFixture]
public class AddNewIncomeTests : IncomesTestBase
{
    [Test]
    public async Task AddNewIncomeCommand_AddsIncome()
    {
        var userId = Guid.NewGuid();
        var command = await CreateAddNewIncomeCommand(userId);

        var incomeId = await FinanceTrackingModule.ExecuteCommandAsync(command);
        var income = await FinanceTrackingModule.ExecuteQueryAsync(new GetIncomeQuery(incomeId, userId));

        Assert.That(income, Is.Not.Null);
        Assert.That(income!.TargetWalletId, Is.EqualTo(command.TargetWalletId));
        Assert.That(income.CategoryId, Is.EqualTo(command.CategoryId));
        Assert.That(income.Amount, Is.EqualTo(command.Amount));
        Assert.That(income.Currency, Is.EqualTo("USD"));
        Assert.That(income.MadeOn, Is.EqualTo(command.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(income.Comment, Is.EqualTo(command.Comment));
        Assert.That(income.Tags, Is.EquivalentTo(command.Tags!));
    }

    [Test]
    public async Task AddNewIncomeCommand_IncreasesBalanceOnCashWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateCashWallet(userId, initialBalance: 100);
        var command = await CreateAddNewIncomeCommand(userId, targetWalletId, amount: 200);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(300));
    }

    [Test]
    public async Task AddNewIncomeCommand_IncreasesBalanceOnDebitWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateDebitWallet(userId, initialBalance: 100);
        var command = await CreateAddNewIncomeCommand(userId, targetWalletId, amount: 200);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.Balance, Is.EqualTo(300));
    }

    [Test]
    public async Task AddNewIncomeCommand_IncreasesBalanceOnCreditWallet()
    {
        var userId = Guid.NewGuid();
        var targetWalletId = await CreateCreditWallet(userId, initialAvailableBalance: 100);
        var command = await CreateAddNewIncomeCommand(userId, targetWalletId, amount: 200);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(targetWalletId, userId));
        Assert.That(wallet!.AvailableBalance, Is.EqualTo(300));
    }

    [Test]
    public async Task AddNewIncomeCommand_UpdatesUserTags()
    {
        var userId = Guid.NewGuid();
        string[] newTags = ["New user tag 1", "New user tag 2"];

        var command = await CreateAddNewIncomeCommand(userId, tags: newTags);

        _ = await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags!.Values, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task AddNewIncomeCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = await CreateAddNewIncomeCommand(userId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewIncomeCommand_WhenTargetWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = await CreateAddNewIncomeCommand(targetWalletId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewIncomeCommand_WhenCategoryWithGivenIdDoesNotExist_ThrowsInvalidCommandException()
    {
        var command = await CreateAddNewIncomeCommand(categoryId: Guid.NewGuid());

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public async Task AddNewIncomeCommand_WhenAmountIsLessOrEqualToZero_ThrowsInvalidCommandException(int amount)
    {
        var command = await CreateAddNewIncomeCommand(amount: amount);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewIncomeCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var command = await CreateAddNewIncomeCommand(madeOn: OptionalParameter.Default);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }
}
