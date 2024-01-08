namespace App.Modules.FinanceTracking.Application.Wallets;

public class ManualBalanceChangeDto
{
    public required string Type { get; init; }

    public required int Amount { get; init; }

    public required string Currency { get; init; }

    public required DateTime MadeOn { get; init; }
}
