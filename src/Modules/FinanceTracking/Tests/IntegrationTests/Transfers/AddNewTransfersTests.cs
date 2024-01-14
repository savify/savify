using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.Application.Users.Tags.GetUserTags;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class AddNewTransfersTests : TransfersTestBase
{
    [Test]
    public async Task AddNewTransferCommand_AddsTransfer()
    {
        var command = await CreateAddNewTransferCommand();

        var transferId = await FinanceTrackingModule.ExecuteCommandAsync(command);
        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetTransferQuery(transferId, command.UserId));

        Assert.That(transfer, Is.Not.Null);
        Assert.That(transfer!.SourceWalletId, Is.EqualTo(command.SourceWalletId));
        Assert.That(transfer.TargetWalletId, Is.EqualTo(command.TargetWalletId));
        Assert.That(transfer.MadeOn, Is.EqualTo(command.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(transfer.Comment, Is.EqualTo(command.Comment));
        Assert.That(transfer.Tags, Is.EquivalentTo(command.Tags!));
        Assert.That(transfer.SourceAmount, Is.EqualTo(command.Amount));
        Assert.That(transfer.SourceCurrency, Is.EqualTo(command.Currency));
        Assert.That(transfer.TargetAmount, Is.EqualTo(command.Amount));
        Assert.That(transfer.TargetCurrency, Is.EqualTo(command.Currency));
        Assert.That(transfer.ExchangeRate, Is.EqualTo(decimal.One));
    }

    [Test]
    public async Task AddNewTransferCommand_UpdatesUserTags()
    {
        string[] newTags = ["New user tag 1", "New user tag 2"];

        var command = await CreateAddNewTransferCommand(tags: newTags);

        _ = await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(command.UserId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags!.Values, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task AddNewTransferCommand_DecreasesBalanceOnSourceWallet_And_IncreasesBalanceOnTargetWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateWallet(userId, initialBalance: 100);
        var targetWalletId = await CreateWallet(userId, initialBalance: 100);
        var command = await CreateAddNewTransferCommand(userId, sourceWalletId, targetWalletId, amount: 50);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var sourceWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        var targetWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));

        Assert.That(sourceWallet!.Balance, Is.EqualTo(50));
        Assert.That(targetWallet!.Balance, Is.EqualTo(150));
    }

    [Test]
    public async Task AddNewTransferCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = await CreateAddNewTransferCommand(userId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewTransferCommand_WhenSourceWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = await CreateAddNewTransferCommand(sourceWalletId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewTransferCommand_WhenTargetWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = await CreateAddNewTransferCommand(targetWalletId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public async Task AddNewTransferCommand_WhenAmountIsLessOrEqualToZero_ThrowsInvalidCommandException(int amount)
    {
        var command = await CreateAddNewTransferCommand(amount: amount);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(null!)]
    [TestCase("")]
    [TestCase("PL")]
    [TestCase("PLNN")]
    public async Task AddNewTransferCommand_WhenCurrencyIsNotIsoFormat_ThrowsInvalidCommandException(string currency)
    {
        var command = await CreateAddNewTransferCommand(currency: currency);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewTransferCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var command = await CreateAddNewTransferCommand(madeOn: OptionalParameter.Default);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }
}
