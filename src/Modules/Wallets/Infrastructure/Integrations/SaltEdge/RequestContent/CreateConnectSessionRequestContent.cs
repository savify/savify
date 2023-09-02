namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.RequestContent;

public class CreateConnectSessionRequestContent
{
    public string CustomerId { get; }

    public string ProviderCode { get; }

    public Consent Consent { get; }

    public Attempt Attempt { get; }

    public bool DisableProviderSearch { get; }

    public bool DailyRefresh { get; }

    public CreateConnectSessionRequestContent(string customerId, string providerCode, Consent consent, Attempt attempt, bool disableProviderSearch = true, bool dailyRefresh = true)
    {
        CustomerId = customerId;
        ProviderCode = providerCode;
        Consent = consent;
        Attempt = attempt;
        DisableProviderSearch = disableProviderSearch;
        DailyRefresh = dailyRefresh;
    }
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

public class Attempt
{
    public string ReturnTo { get; }

    public string Locale { get; }

    public bool FetchedAccountsNotify { get; }
    
    public CustomFields CustomFields { get; }

    public Attempt(Guid bankConnectionProcessId, string returnTo, string locale = "en", bool fetchedAccountsNotify = true)
    {
        ReturnTo = returnTo;
        Locale = locale;
        FetchedAccountsNotify = fetchedAccountsNotify;
        CustomFields = new CustomFields(bankConnectionProcessId);
    }
}

public class CustomFields
{
    public Guid BankConnectionProcessId { get; }

    public CustomFields(Guid bankConnectionProcessId)
    {
        BankConnectionProcessId = bankConnectionProcessId;
    }
}
