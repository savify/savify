namespace App.Modules.Transactions.Domain.Transactions;

public record Recipient
{
    public string Address { get; }

    private Recipient(string address)
    {
        Address = address;
    }

    internal static Recipient WhoHave(string address) => new(address);
}
