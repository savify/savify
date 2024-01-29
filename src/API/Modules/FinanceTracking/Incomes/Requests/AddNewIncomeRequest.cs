namespace App.API.Modules.FinanceTracking.Incomes.Requests;

public class AddNewIncomeRequest
{
    public Guid TargetWalletId { get; set; }

    public Guid CategoryId { get; set; }

    public int Amount { get; set; }

    public DateTime MadeOn { get; set; }

    public string? Comment { get; set; }

    public IEnumerable<string>? Tags { get; set; }
}
