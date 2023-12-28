﻿using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalValues;
using App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using RazorEngineCore;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class AddNewTransferTests : TestBase
{
    [Test]
    public async Task AddNewTransferCommand_TransferIsAdded()
    {
        var command = CreateCommand();

        var transferId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetTransferQuery(transferId));

        Assert.That(transfer, Is.Not.Null);
        Assert.That(transfer.SourceWalletId, Is.EqualTo(command.SourceWalletId));
        Assert.That(transfer.TargetWalletId, Is.EqualTo(command.TargetWalletId));
        Assert.That(transfer.Amount, Is.EqualTo(command.Amount));
        Assert.That(transfer.Currency, Is.EqualTo(command.Currency));
        Assert.That(transfer.MadeOn, Is.EqualTo(command.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(transfer.Comment, Is.EqualTo(command.Comment));
        Assert.That(transfer.Tags, Is.EquivalentTo(command.Tags!));
    }

    [Test]
    public async Task AddNewTransferCommand_UserTagsAreUpdated()
    {
        var userId = Guid.NewGuid();
        string[] newTags = ["New user tag 1", "New user tag 2"];

        var command = CreateCommand(userId: userId, tags: newTags);

        var _ = await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags.Tags, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task AddNewTransferCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = CreateCommand(userId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewTransferCommand_WhenSourceWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = CreateCommand(sourceWalletId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewTransferCommand_WhenTargetWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var command = CreateCommand(targetWalletId: Guid.Empty);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public async Task AddNewTransferCommand_WhenAmountIsLessOrEqualToZero_ThrowsInvalidCommandException(int amount)
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
    public async Task AddNewTransferCommand_WhenCurrencyIsNotIsoFormat_ThrowsInvalidCommandException(string currency)
    {
        var command = CreateCommand(currency: currency);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task AddNewTransferCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var command = CreateCommand(madeOn: OptionalParameter.Default);

        var act = () => FinanceTrackingModule.ExecuteCommandAsync(command);

        await Assert.ThatAsync(act, Throws.TypeOf<InvalidCommandException>());
    }

    private AddNewTransferCommand CreateCommand(
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> sourceWalletId = default,
        OptionalParameter<Guid> targetWalletId = default,
        OptionalParameter<int> amount = default,
        OptionalParameter<string> currency = default,
        OptionalParameter<DateTime> madeOn = default,
        OptionalParameter<string> comment = default,
        OptionalParameter<IEnumerable<string>> tags = default)
    {
        return new AddNewTransferCommand(
            userId.GetValueOr(Guid.NewGuid()),
            sourceWalletId.GetValueOr(Guid.NewGuid()),
            targetWalletId.GetValueOr(Guid.NewGuid()),
            amount.GetValueOr(100),
            currency.GetValueOr("USD"),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Savings transfer"),
            tags.GetValueOr(["Savings", "Minor"]));
    }
}
