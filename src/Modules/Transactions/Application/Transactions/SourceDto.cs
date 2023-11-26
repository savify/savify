namespace App.Modules.Transactions.Application.Transactions;

public class SourceDto
{
    public SourceDto(string senderAddress, int amount, string currency)
    {
        SenderAddress = senderAddress;
        Amount = amount;
        Currency = currency;
    }

    public SourceDto()
    { }

    public string SenderAddress { get; set; }
    public int Amount { get; set; }
    public string Currency { get; set; }
}
