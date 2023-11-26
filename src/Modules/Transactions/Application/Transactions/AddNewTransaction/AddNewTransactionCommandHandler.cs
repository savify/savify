using App.Modules.Transactions.Application.Configuration.Commands;
using App.Modules.Transactions.Domain.Finance;
using App.Modules.Transactions.Domain.Transactions;

namespace App.Modules.Transactions.Application.Transactions.AddNewTransaction;
internal class AddNewTransactionCommandHandler : ICommandHandler<AddNewTransactionCommand, Guid>
{
    private readonly ITransactionsRepository _repository;

    public AddNewTransactionCommandHandler(ITransactionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddNewTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = CreateTransaction(request);

        await _repository.AddAsync(transaction);

        return transaction.Id.Value;
    }

    private Transaction CreateTransaction(AddNewTransactionCommand request)
    {
        var type = ToTransactionType(request.Type);
        var source = Source.With(Sender.WhoHas(request.Source.SenderAddress),
                                 Money.From(request.Source.Amount, Currency.From(request.Source.Currency)));

        var target = Target.With(Recipient.WhoHas(request.Target.RecipientAddress),
                                 Money.From(request.Target.Amount, Currency.From(request.Target.Currency)));

        var transaction = Transaction.AddNew(type, source, target, request.MadeOn, request.Comment, request.Tags);
        return transaction;
    }

    private TransactionType ToTransactionType(TransactionTypeDto dto) => dto switch
    {
        TransactionTypeDto.Expense => TransactionType.Expense(),
        TransactionTypeDto.Income => TransactionType.Income(),
        TransactionTypeDto.Transfer => TransactionType.Transfer(),
        _ => throw new Exception($"Unknown value of {nameof(TransactionTypeDto)}: {dto}")
    };
}
