namespace App.Modules.Transactions.Domain.Transactions;

public record TransactionType(string Value)
{
    public static TransactionType Expense() => new TransactionType(nameof(Expense));

    public static TransactionType Income() => new TransactionType(nameof(Income));

    public static TransactionType Transfer() => new TransactionType(nameof(Transfer));
}

