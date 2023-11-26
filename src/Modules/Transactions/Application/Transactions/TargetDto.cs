namespace App.Modules.Transactions.Application.Transactions;

public class TargetDto
{
    public TargetDto(string recipientAddress, int amount, string currency)
    {
        RecipientAddress = recipientAddress;
        Amount = amount;
        Currency = currency;
    }

    public TargetDto()
    { }
    public string RecipientAddress { get; set; }
    public int Amount { get; set; }
    public string Currency { get; set; }
}
