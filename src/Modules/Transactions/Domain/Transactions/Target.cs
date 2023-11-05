using App.Modules.Transactions.Domain.Finance;

namespace App.Modules.Transactions.Domain.Transactions;

public record Target
{
    public Recipient Recipient { get; }

    public Money Amount { get; }

    private Target(Recipient recipient, Money amount)
    {
        Recipient = recipient;
        Amount = amount;
    }

    public static Target With(Recipient recipient, Money amount) => new(recipient, amount);
}
