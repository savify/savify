namespace App.Modules.Wallets.Application.Wallets.WalletsViewMetadata;

public class WalletViewMetadataDto
{
    public Guid WalletId { get; set; }

    public string? Color { get; set; }

    public string? Icon { get; set; }

    public bool IsConsideredInTotalBalance { get; set; }
}
