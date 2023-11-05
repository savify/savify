namespace App.Modules.Transactions.Domain.Transactions;

public record TransactionType
{
    private readonly string _value;

    private TransactionType(string value)
    {
        _value = value;
    }

    internal static TransactionType Expense() => new TransactionType(nameof(Expense));

    internal static TransactionType Income() => new TransactionType(nameof(Income));

    internal static TransactionType Transfer() => new TransactionType(nameof(Transfer));
}

