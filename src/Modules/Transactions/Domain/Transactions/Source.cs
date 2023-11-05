using App.Modules.Transactions.Domain.Finance;

namespace App.Modules.Transactions.Domain.Transactions;

public record Source
{
    public Sender Sender { get; }

    public Money Amount { get; }

    private Source(Sender sender, Money amount)
    {
        Sender = sender;
        Amount = amount;
    }

    internal static Source With(Sender sender, Money amount) => new(sender, amount);
}
