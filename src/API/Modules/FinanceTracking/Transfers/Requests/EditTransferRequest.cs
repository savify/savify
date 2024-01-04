namespace App.API.Modules.FinanceTracking.Transfers.Requests;

public class EditTransferRequest
{
    public Guid SourceWalletId { get; set; }

    public Guid TargetWalletId { get; set; }

    public int Amount { get; set; }

    public string Currency { get; set; }

    public DateTime MadeOn { get; set; }

    public string? Comment { get; set; }

    public IEnumerable<string>? Tags { get; set; }
}
