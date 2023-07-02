namespace App.API.Modules.Accounts.CreditAccounts;

public class AddNewCreditAccountRequest
{
    public string Title { get; set; }

    public string Currency { get; set; }

    public int AvailableBalance { get; set; }

    public int CreditLimit { get; set; }
}