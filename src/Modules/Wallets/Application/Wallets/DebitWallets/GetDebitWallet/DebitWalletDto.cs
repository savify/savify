using App.Modules.Wallets.Application.Wallets.WalletsViewMetadata;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.GetDebitWallet;

public class DebitWalletDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Title { get; set; }

    public string Currency { get; set; }

    public int Balance { get; set; }

    public WalletViewMetadataDto ViewMetadata { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsRemoved { get; set; }
}
