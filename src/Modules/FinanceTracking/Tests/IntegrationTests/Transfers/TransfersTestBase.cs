using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
using App.Modules.FinanceTracking.Application.Transfers.EditTransfer;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

public class TransfersTestBase : TestBase
{
    protected async Task<AddNewTransferCommand> CreateAddNewTransferCommand(
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> sourceWalletId = default,
        OptionalParameter<Guid> targetWalletId = default,
        OptionalParameter<string> targetWalletCurrency = default,
        OptionalParameter<int> sourceAmount = default,
        OptionalParameter<int?> targetAmount = default,
        OptionalParameter<DateTime> madeOn = default,
        OptionalParameter<string> comment = default,
        OptionalParameter<IEnumerable<string>> tags = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());

        return new AddNewTransferCommand(
            userIdValue,
            sourceWalletId.GetValueOr(await CreateWallet(userIdValue)),
            targetWalletId.GetValueOr(await CreateWallet(userIdValue, currency: targetWalletCurrency.GetValueOr("USD"))),
            sourceAmount.GetValueOr(100),
            targetAmount.GetValueOr(null),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Savings transfer"),
            tags.GetValueOr(["Savings", "Minor"]));
    }

    protected async Task<Guid> AddNewTransferAsync(
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> sourceWalletId = default,
        OptionalParameter<Guid> targetWalletId = default,
        OptionalParameter<int> sourceAmount = default,
        OptionalParameter<int?> targetAmount = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());
        var sourceWalletIdValue = sourceWalletId.GetValueOr(await CreateWallet(userIdValue));
        var targetWalletIdValue = targetWalletId.GetValueOr(await CreateWallet(userIdValue));

        var command = new AddNewTransferCommand(
            userId: userIdValue,
            sourceWalletId: sourceWalletIdValue,
            targetWalletId: targetWalletIdValue,
            sourceAmount: sourceAmount.GetValueOr(100),
            targetAmount: targetAmount.GetValueOr(null),
            madeOn: DateTime.UtcNow,
            comment: "Savings transfer",
            tags: ["Savings", "Minor"]);

        var transferId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return transferId;
    }

    protected async Task<EditTransferCommand> CreateEditTransferCommand(
        Guid transferId,
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> sourceWalletId = default,
        OptionalParameter<Guid> targetWalletId = default,
        OptionalParameter<string> targetCurrency = default,
        OptionalParameter<int> sourceAmount = default,
        OptionalParameter<int?> targetAmount = default,
        OptionalParameter<DateTime> madeOn = default,
        OptionalParameter<string> comment = default,
        OptionalParameter<IEnumerable<string>> tags = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());

        return new EditTransferCommand(
            transferId,
            userIdValue,
            sourceWalletId.GetValueOr(await CreateWallet(userIdValue)),
            targetWalletId.GetValueOr(await CreateWallet(userIdValue, currency: targetCurrency.GetValueOr("USD"))),
            sourceAmount.GetValueOr(500),
            targetAmount.GetValueOr(null),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Edited transfer"),
            tags.GetValueOr(["Edited"]));
    }

    protected async Task<Guid> CreateWallet(Guid userId, int initialBalance = 100, string currency = "USD")
    {
        return await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId.Equals(Guid.Empty) ? Guid.NewGuid() : userId,
            "Cash wallet",
            currency,
            initialBalance,
            "#000000",
            "https://cdn.savify.io/icons/icon.svg",
            true));
    }
}
