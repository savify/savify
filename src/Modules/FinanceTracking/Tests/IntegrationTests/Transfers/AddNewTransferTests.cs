using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class AddNewTransferTests : TestBase
{
    [Test]
    public async Task AddNewTransferCommand_Tests()
    {
        var command = new AddNewTransferCommand(
            sourceWalletId: Guid.NewGuid(),
            targetWalletId: Guid.NewGuid(),
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Savings transfer",
            tags: ["Savings", "Minor"]);

        var transferId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetTransferQuery(transferId));

        Assert.That(transfer, Is.Not.Null);
        Assert.That(transfer.SourceWalletId, Is.EqualTo(command.SourceWalletId));
        Assert.That(transfer.TargetWalletId, Is.EqualTo(command.TargetWalletId));
        Assert.That(transfer.Amount, Is.EqualTo(command.Amount));
        Assert.That(transfer.Currency, Is.EqualTo(command.Currency));
        Assert.That(transfer.MadeOn, Is.EqualTo(command.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(transfer.Comment, Is.EqualTo(command.Comment));
        Assert.That(transfer.Tags, Is.EquivalentTo(command.Tags));
    }

    [Test]
    public async Task AddNewTransferCommand_WhenSourceWalletIdIsDefaultGuid_ThrowsInvalidCommandException()
    {
        var command = new AddNewTransferCommand(
            sourceWalletId: Guid.Empty,
            targetWalletId: Guid.NewGuid(),
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Savings transfer",
            tags: ["Savings", "Minor"]);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewTransferCommand_WhenTargetWalletIdIsDefaultGuid_ThrowsInvalidCommandException()
    {
        var command = new AddNewTransferCommand(
            sourceWalletId: Guid.NewGuid(),
            targetWalletId: Guid.Empty,
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Savings transfer",
            tags: ["Savings", "Minor"]);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public async Task AddNewTransferCommand_WhenAmountIsLessOrEqualToZero_ThrowsInvalidCommandException(int amount)
    {
        var command = new AddNewTransferCommand(
            sourceWalletId: Guid.NewGuid(),
            targetWalletId: Guid.NewGuid(),
            amount: amount,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Savings transfer",
            tags: ["Savings", "Minor"]);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase("PL")]
    [TestCase("PLNN")]
    public async Task AddNewTransferCommand_WhenCurrencyIsNotIsoFormat_ThrowsInvalidCommandException(string currency)
    {
        var command = new AddNewTransferCommand(
            sourceWalletId: Guid.NewGuid(),
            targetWalletId: Guid.Empty,
            amount: 100,
            currency: currency,
            madeOn: DateTime.UtcNow,
            comment: "Savings transfer",
            tags: ["Savings", "Minor"]);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewTransferCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var command = new AddNewTransferCommand(
            sourceWalletId: Guid.NewGuid(),
            targetWalletId: Guid.Empty,
            amount: 100,
            currency: "USD",
            madeOn: default,
            comment: "Savings transfer",
            tags: ["Savings", "Minor"]);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewTransferCommand_WhenCommentIsNull_ThrowsInvalidCommandException()
    {
        var command = new AddNewTransferCommand(
            sourceWalletId: Guid.NewGuid(),
            targetWalletId: Guid.Empty,
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: null!,
            tags: ["Savings", "Minor"]);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewTransferCommand_WhenTagsCollectionIsNull_ThrowsInvalidCommandException()
    {
        var command = new AddNewTransferCommand(
            sourceWalletId: Guid.NewGuid(),
            targetWalletId: Guid.Empty,
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Savings transfer",
            tags: null!);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }
}
