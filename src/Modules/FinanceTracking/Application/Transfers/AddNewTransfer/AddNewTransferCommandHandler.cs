using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;

internal class AddNewTransferCommandHandler : ICommandHandler<AddNewTransferCommand, Guid>
{
    private readonly ITransfersRepository _repository;

    public AddNewTransferCommandHandler(ITransfersRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddNewTransferCommand request, CancellationToken cancellationToken)
    {
        var transfer = Transfer.AddNew(
            new WalletId(request.SourceWalletId),
            new WalletId(request.TargetWalletId),
            Money.From(request.Amount, request.Currency),
            request.MadeOn,
            request.Comment,
            request.Tags);

        await _repository.AddAsync(transfer);

        return transfer.Id.Value;
    }
}
