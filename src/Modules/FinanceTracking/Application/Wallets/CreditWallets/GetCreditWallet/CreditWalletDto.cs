using App.Modules.FinanceTracking.Application.Wallets.WalletsViewMetadata;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;

public class CreditWalletDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public required string Title { get; init; }

    public int AvailableBalance { get; set; }

    public int CreditLimit { get; init; }

    public required string Currency { get; init; }

    public bool IsRemoved { get; init; }

    public required WalletViewMetadataDto ViewMetadata { get; set; }

    public IEnumerable<ManualBalanceChangeDto> ManualBalanceChanges { get; set; } = new List<ManualBalanceChangeDto>();
}
