using App.Modules.Accounts.Application.AccountsViewMetadata;

namespace App.Modules.Accounts.Application.CreditAccounts.GetCreditAccount;

public class CreditAccountDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int AvailableBalance { get; set; }
    public int CreditLimit { get; set; }
    public string Currency { get; set; }
    public AccountViewMetadataDto ViewMetadata { get; set; }
    public DateTime CreatedAt { get; set; }
}
