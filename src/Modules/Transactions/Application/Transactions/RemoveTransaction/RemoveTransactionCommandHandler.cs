using App.Modules.Transactions.Application.Configuration.Commands;
using App.Modules.Transactions.Domain.Transactions;

namespace App.Modules.Transactions.Application.Transactions.RemoveTransaction;

internal class RemoveTransactionCommandHandler : ICommandHandler<RemoveTransactionCommand>
{
    private readonly ITransactionsRepository _repository;

    public RemoveTransactionCommandHandler(ITransactionsRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(new TransactionId(request.TransactionId));

        transaction.Remove(_repository);
    }
}
