using App.BuildingBlocks.Domain;

namespace App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;

public class AccountViewMetadata : Entity, IAggregateRoot
{
    public AccountId AccountId { get; private set; }
    
    public string? Color { get; private set; }
    
    public string? Icon { get; private set; }
    
    public bool IsConsideredInTotalBalance { get; private set; }

    public static AccountViewMetadata CreateForAccount(AccountId accountId, string color, string icon, bool isConsideredInTotalBalance)
    {
        return new AccountViewMetadata(accountId, color, icon, isConsideredInTotalBalance);
    }

    public static AccountViewMetadata CreateDefaultForAccount(AccountId accountId)
    {
        return CreateForAccount(accountId, color: null, icon: null, isConsideredInTotalBalance: true);
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
