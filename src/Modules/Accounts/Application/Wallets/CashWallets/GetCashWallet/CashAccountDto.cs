using App.Modules.Wallets.Application.Wallets.WalletViewMetadata;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.GetCashWallet;

public class CashWalletDto
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public string Title { get; set; }
    
    public string Currency { get; set; }
    
    public int Balance { get; set; }
    
    public WalletViewMetadataDto ViewMetadata { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
