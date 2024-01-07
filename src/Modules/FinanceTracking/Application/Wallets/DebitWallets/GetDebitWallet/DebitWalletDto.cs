using App.Modules.FinanceTracking.Application.Wallets.WalletsViewMetadata;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;

public class DebitWalletDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public required string Title { get; init; }

    public required string Currency { get; init; }

    public int Balance { get; set; }

    public required WalletViewMetadataDto ViewMetadata { get; set; }

    public bool IsRemoved { get; init; }

    public Guid? BankConnectionId { get; init; }

    public Guid? BankAccountId { get; init; }
}
