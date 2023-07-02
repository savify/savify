namespace App.API.Modules.Accounts.DebitAccounts;

public class AddNewDebitAccountRequest
{
    public string Title { get; set; }

    public string Currency { get; set; }

    public int Balance { get; set; }
}