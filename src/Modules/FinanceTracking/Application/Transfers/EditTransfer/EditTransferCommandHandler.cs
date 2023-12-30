using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Transfers.EditTransfer;

internal class EditTransferCommandHandler(ITransfersRepository repository) : ICommandHandler<EditTransferCommand>
{
    public async Task Handle(EditTransferCommand command, CancellationToken cancellationToken)
    {
        var transfer = await repository.GetByIdAsync(new TransferId(command.TransferId));

        transfer.Edit(
            new UserId(command.UserId),
            new WalletId(command.SourceWalletId),
            new WalletId(command.TargetWalletId),
            Money.From(command.Amount, command.Currency),
            command.MadeOn,
            command.Comment,
            command.Tags);
    }
}
