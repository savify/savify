using App.Modules.Wallets.Application.Wallets.WalletsViewMetadata;

namespace App.Modules.Wallets.Application.Wallets.CreditWallets.GetCreditWallet;

public class CreditWalletDto
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public int AvailableBalance { get; set; }
    
    public int CreditLimit { get; set; }
    
    public string Currency { get; set; }
    
    public WalletViewMetadataDto ViewMetadata { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
