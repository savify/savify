using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;

internal class AddNewTransferCommandHandler(ITransferRepository transferRepository, IWalletsRepository walletsRepository, TransactionAmountFactory transactionAmountFactory) : ICommandHandler<AddNewTransferCommand, Guid>
{
    public async Task<Guid> Handle(AddNewTransferCommand command, CancellationToken cancellationToken)
    {
        var sourceWalletId = new WalletId(command.SourceWalletId);
        var targetWalletId = new WalletId(command.TargetWalletId);
        var userId = new UserId(command.UserId);

        var sourceWallet = await walletsRepository.GetByWalletIdAndUserIdAsync(sourceWalletId, userId);
        var targetWallet = await walletsRepository.GetByWalletIdAndUserIdAsync(targetWalletId, userId);

        var transactionAmount = await transactionAmountFactory.CreateAsync(
            command.SourceAmount,
            sourceWallet.Currency,
            command.TargetAmount,
            targetWallet.Currency,
            command.MadeOn);

        var transfer = Transfer.AddNew(
            new UserId(command.UserId),
            sourceWallet,
            targetWallet,
            transactionAmount,
            command.MadeOn,
            command.Comment,
            command.Tags);

        await transferRepository.AddAsync(transfer);

        return transfer.Id.Value;
    }
}
