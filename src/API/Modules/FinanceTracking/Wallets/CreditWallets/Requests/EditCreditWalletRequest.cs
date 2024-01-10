namespace App.API.Modules.FinanceTracking.Wallets.CreditWallets.Requests;

public class EditCreditWalletRequest
{
    public string? Title { get; set; }

    public string? Currency { get; set; }

    public int? AvailableBalance { get; set; }

    public int? CreditLimit { get; set; }

    public string? Color { get; set; }

    public string? Icon { get; set; }

    public bool? ConsiderInTotalBalance { get; set; }
}
