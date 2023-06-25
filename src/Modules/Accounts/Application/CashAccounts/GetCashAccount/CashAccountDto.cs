namespace App.Modules.Accounts.Application.CashAccounts.GetCashAccount;
public class CashAccountDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Currency { get; set; }
    public int Balance { get; set; }
    public DateTime CreatedAt { get; set; }
}
