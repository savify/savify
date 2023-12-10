namespace App.Modules.FinanceTracking.IntegrationTests.BankConnectionProcessing;

public static class BankConnectionProcessingData
{
    public static Guid UserId = Guid.Parse("d247ae50-8ea5-441b-9d30-104ebbca0ad8");

    public static Guid BankId = Guid.Parse("16b85058-cd7d-4571-a768-d45bec4bba11");

    public static string CountryCode = "XF";

    public static string ExternalProviderCode = "fakebank_interactive_xf";

    public static string ExternalCustomerId = "1087222023010130820";

    public static string ExternalConnectionId = "1092933278672886687";

    public static string ExternalConsentId = "1092933279444638626";

    public static string ExpectedRedirectUrl = "https://www.saltedge.com/connect?token=GENERATED_TOKEN";

    public static string ExternalUSDAccountId = "1092933677509253511";

    public static double ExternalUSDAccountBalance = 1000.00;

    public static string ExternalUSDAccountCurrency = "USD";

    public static string ExternalPLNAccountId = "1092933677433756038";

    public static double ExternalPLNAccountBalance = 2000.00;

    public static string ExternalPLNAccountCurrency = "PLN";
}
