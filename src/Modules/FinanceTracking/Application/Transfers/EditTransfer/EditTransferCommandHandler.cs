using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Transfers.EditTransfer;

internal class EditTransferCommandHandler(ITransfersRepository repository) : ICommandHandler<EditTransferCommand>
{
    public async Task Handle(EditTransferCommand request, CancellationToken cancellationToken)
    {
        var transfer = await repository.GetByIdAsync(new TransferId(request.TransferId));

        transfer.Edit(
            new UserId(request.UserId),
            new WalletId(request.SourceWalletId),
            new WalletId(request.TargetWalletId),
            Money.From(request.Amount, request.Currency),
            request.MadeOn,
            request.Comment,
            request.Tags);
    }
}
