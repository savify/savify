namespace App.API.Modules.FinanceTracking.ExchangeRates.Requests;

public class GetCurrentExchangeRateRequest
{
    public required string From { get; set; }

    public required string To { get; set; }
}
