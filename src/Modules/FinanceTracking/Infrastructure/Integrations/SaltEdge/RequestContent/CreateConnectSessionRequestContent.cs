namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.RequestContent;

public class CreateConnectSessionRequestContent(
    string customerId,
    string providerCode,
    Consent consent,
    Attempt attempt,
    bool disableProviderSearch = true,
    bool dailyRefresh = true)
{
    public string CustomerId { get; } = customerId;

    public string ProviderCode { get; } = providerCode;

    public Consent Consent { get; } = consent;

    public Attempt Attempt { get; } = attempt;

    public bool DisableProviderSearch { get; } = disableProviderSearch;

    public bool DailyRefresh { get; } = dailyRefresh;
}

public class Consent
{
    public string[] Scopes { get; }

    public static Consent Default => new(new[] { "account_details", "transactions_details" });

    private Consent(string[] scopes)
    {
        Scopes = scopes;
    }
}

public class Attempt(
    Guid bankConnectionProcessId,
    string returnTo,
    string locale,
    bool fetchedAccountsNotify = true)
{
    public string ReturnTo { get; } = returnTo;

    public string Locale { get; } = locale;

    public bool FetchedAccountsNotify { get; } = fetchedAccountsNotify;

    public CustomFields CustomFields { get; } = new(bankConnectionProcessId);
}

public class CustomFields(Guid bankConnectionProcessId)
{
    public Guid BankConnectionProcessId { get; } = bankConnectionProcessId;
}
