namespace App.Modules.Transactions.Domain.Transactions;

public record Sender
{
    public string Address { get; }

    private Sender(string address)
    {
        Address = address;
    }

    internal static Sender WhoHave(string address) => new(address);
}
