using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Transfers.EditTransfer;

internal class EditTransferCommandHandler(ITransferRepository transferRepository, IWalletsRepository walletsRepository, TransferAmountFactory transferAmountFactory) : ICommandHandler<EditTransferCommand>
{
    public async Task Handle(EditTransferCommand command, CancellationToken cancellationToken)
    {
        var sourceWalletId = new WalletId(command.SourceWalletId);
        var targetWalletId = new WalletId(command.TargetWalletId);
        var userId = new UserId(command.UserId);

        var sourceWallet = await walletsRepository.GetByWalletIdAndUserIdAsync(sourceWalletId, userId);
        var targetWallet = await walletsRepository.GetByWalletIdAndUserIdAsync(targetWalletId, userId);

        var transfer = await transferRepository.GetByIdAndUserIdAsync(new TransferId(command.TransferId), new UserId(command.UserId));

        var transactionAmount = await transferAmountFactory.CreateAsync(
            command.SourceAmount,
            sourceWallet.Currency,
            command.TargetAmount,
            targetWallet.Currency,
            command.MadeOn);

        transfer.Edit(
            sourceWallet,
            targetWallet,
            transactionAmount,
            command.MadeOn,
            command.Comment,
            command.Tags);
    }
}
