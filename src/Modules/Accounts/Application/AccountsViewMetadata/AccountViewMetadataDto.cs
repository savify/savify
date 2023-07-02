namespace App.Modules.Accounts.Application.AccountsViewMetadata;
public class AccountViewMetadataDto
{
    public Guid AccountId { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public bool IsConsideredInTotalBalance { get; set; }
}
