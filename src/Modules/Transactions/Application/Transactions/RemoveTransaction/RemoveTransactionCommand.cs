using App.Modules.Transactions.Application.Contracts;

namespace App.Modules.Transactions.Application.Transactions.RemoveTransaction;

public class RemoveTransactionCommand : CommandBase
{
    public Guid TransactionId { get; }

    public RemoveTransactionCommand(Guid transactionId)
    {
        TransactionId = transactionId;
    }
}
