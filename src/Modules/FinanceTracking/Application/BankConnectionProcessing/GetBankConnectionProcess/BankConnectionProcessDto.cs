namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.GetBankConnectionProcess;

public class BankConnectionProcessDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid BankId { get; set; }

    public Guid WalletId { get; set; }

    public string WalletType { get; set; }

    public string Status { get; set; }

    public string RedirectUrl { get; set; }

    public DateTime RedirectUrlExpiresAt { get; set; }

    public DateTime InitiatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
