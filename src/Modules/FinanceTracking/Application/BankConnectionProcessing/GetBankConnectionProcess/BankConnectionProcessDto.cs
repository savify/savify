namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.GetBankConnectionProcess;

public class BankConnectionProcessDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public Guid BankId { get; init; }

    public Guid WalletId { get; init; }

    public required string WalletType { get; init; }

    public required string Status { get; init; }

    public required string RedirectUrl { get; init; }

    public DateTime RedirectUrlExpiresAt { get; init; }

    public DateTime InitiatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
