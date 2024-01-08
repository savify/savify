using App.Modules.FinanceTracking.Application.Wallets.WalletsViewMetadata;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;

public class CashWalletDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public required string Title { get; init; }

    public required string Currency { get; init; }

    public int Balance { get; set; }

    public bool IsRemoved { get; init; }

    public required WalletViewMetadataDto ViewMetadata { get; set; }

    public IEnumerable<ManualBalanceChangeDto> ManualBalanceChanges { get; set; } = new List<ManualBalanceChangeDto>();
}
