namespace App.API.Modules.FinanceTracking.Transfers.Requests;

public class AddNewTransferRequest
{
    public Guid SourceWalletId { get; set; }

    public Guid TargetWalletId { get; set; }

    public int SourceAmount { get; set; }

    public int? TargetAmount { get; set; }

    public DateTime MadeOn { get; set; }

    public string? Comment { get; set; }

    public IEnumerable<string>? Tags { get; set; }
}
