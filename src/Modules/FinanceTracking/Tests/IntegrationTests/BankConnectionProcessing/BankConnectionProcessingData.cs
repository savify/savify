namespace App.Modules.FinanceTracking.IntegrationTests.BankConnectionProcessing;

public static class BankConnectionProcessingData
{
    public static Guid UserId = Guid.Parse("d247ae50-8ea5-441b-9d30-104ebbca0ad8");

    public static Guid BankId = Guid.Parse("16b85058-cd7d-4571-a768-d45bec4bba11");

    public static readonly string CountryCode = "XF";

    public static readonly string ExternalProviderCode = "fakebank_interactive_xf";

    public static readonly string ExternalCustomerId = "1087222023010130820";

    public static readonly string ExternalConnectionId = "1092933278672886687";

    public static readonly string ExternalConsentId = "1092933279444638626";

    public static readonly string ExpectedRedirectUrl = "https://www.saltedge.com/connect?token=GENERATED_TOKEN";

    public static readonly string ExternalUsdAccountId = "1092933677509253511";

    public static readonly double ExternalUsdAccountBalance = 1000.00;

    public static readonly string ExternalUsdAccountCurrency = "USD";

    public static readonly string ExternalPlnAccountId = "1092933677433756038";

    public static readonly double ExternalPlnAccountBalance = 2000.00;

    public static readonly string ExternalPlnAccountCurrency = "PLN";
}
