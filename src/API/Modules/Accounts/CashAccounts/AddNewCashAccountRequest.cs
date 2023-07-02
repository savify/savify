namespace App.API.Modules.Accounts.CashAccounts;

public class AddNewCashAccountRequest
{
    public string Title { get; set; }

    public string Currency { get; set; }

    public int Balance { get; set; }
}