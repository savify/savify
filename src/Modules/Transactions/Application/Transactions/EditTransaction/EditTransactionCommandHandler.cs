using App.Modules.Transactions.Application.Configuration.Commands;
using App.Modules.Transactions.Domain.Finance;
using App.Modules.Transactions.Domain.Transactions;

namespace App.Modules.Transactions.Application.Transactions.EditTransaction;

internal class EditTransactionCommandHandler : ICommandHandler<EditTransactionCommand>
{
    private readonly ITransactionsRepository _repository;

    public EditTransactionCommandHandler(ITransactionsRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(EditTransactionCommand request, CancellationToken cancellationToken)
    {
        var type = ToTransactionType(request.Type);
        var source = Source.With(Sender.WhoHas(request.Source.SenderAddress),
                                 Money.From(request.Source.Amount, Currency.From(request.Source.Currency)));

        var target = Target.With(Recipient.WhoHas(request.Target.RecipientAddress),
                                 Money.From(request.Target.Amount, Currency.From(request.Target.Currency)));

        var transaction = await _repository.GetByIdAsync(new TransactionId(request.TransactionId));
        transaction.Edit(type, source, target, request.MadeOn, request.Comment, request.Tags);
    }

    private TransactionType ToTransactionType(TransactionTypeDto dto) => dto switch
    {
        TransactionTypeDto.Expense => TransactionType.Expense(),
        TransactionTypeDto.Income => TransactionType.Income(),
        TransactionTypeDto.Transfer => TransactionType.Transfer(),
        _ => throw new Exception($"Unknown value of {nameof(TransactionTypeDto)}: {dto}")
    };
}
