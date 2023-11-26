using App.Modules.Transactions.Application.Contracts;

namespace App.Modules.Transactions.Application.Transactions.GetTransaction;

public class GetTransactionQuery : QueryBase<TransactionDto?>
{
    public Guid TransactionId { get; }

    public GetTransactionQuery(Guid transactionId)
    {
        TransactionId = transactionId;
    }
}
