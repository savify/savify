namespace App.Modules.Transactions.Domain.Transactions;

public record Sender
{
    public string Address { get; }

    private Sender(string address)
    {
        Address = address;
    }

    public static Sender WhoHas(string address) => new(address);
}
