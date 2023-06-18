using App.BuildingBlocks.Domain;

namespace App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;

public class AccountViewMetadata : Entity
{
    public AccountId AccountId { get; private set; }
    
    public string? Color { get; private set; }
    
    public string? Icon { get; private set; }
    
    public bool IsConsideredInTotalBalance { get; private set; }

    public static AccountViewMetadata CreateForAccount(AccountId accountId, string? color = null, string? icon = null,
        bool isConsideredInTotalBalance = true)
    {
        return new AccountViewMetadata(accountId, color, icon, isConsideredInTotalBalance);
    }
    
    private AccountViewMetadata(AccountId accountId, string? color, string? icon, bool isConsideredInTotalBalance)
    {
        AccountId = accountId;
        Color = color;
        Icon = icon;
        IsConsideredInTotalBalance = isConsideredInTotalBalance;
    }

    private AccountViewMetadata() { }
}
