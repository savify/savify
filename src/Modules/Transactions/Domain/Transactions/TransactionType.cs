namespace App.Modules.Transactions.Domain.Transactions;

public record TransactionType(string Value)
{
    internal static TransactionType Expense() => new TransactionType(nameof(Expense));

    internal static TransactionType Income() => new TransactionType(nameof(Income));

    internal static TransactionType Transfer() => new TransactionType(nameof(Transfer));
}

