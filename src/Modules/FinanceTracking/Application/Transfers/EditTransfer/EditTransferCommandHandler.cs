using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Transfers.EditTransfer;

internal class EditTransferCommandHandler(ITransferRepository transferRepository, IWalletsRepository walletsRepository) : ICommandHandler<EditTransferCommand>
{
    public async Task Handle(EditTransferCommand command, CancellationToken cancellationToken)
    {
        var transfer = await transferRepository.GetByIdAndUserIdAsync(new TransferId(command.TransferId), new UserId(command.UserId));

        transfer.Edit(
            new WalletId(command.SourceWalletId),
            new WalletId(command.TargetWalletId),
            Money.From(command.Amount, command.Currency),
            command.MadeOn,
            walletsRepository,
            command.Comment,
            command.Tags);
    }
}
