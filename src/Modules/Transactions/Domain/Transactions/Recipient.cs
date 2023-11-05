namespace App.Modules.Transactions.Domain.Transactions;

public record Recipient
{
    public string Address { get; }

    private Recipient(string address)
    {
        Address = address;
    }

    public static Recipient WhoHas(string address) => new(address);
}
