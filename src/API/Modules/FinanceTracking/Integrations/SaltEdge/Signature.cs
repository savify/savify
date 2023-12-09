namespace App.API.Modules.FinanceTracking.Integrations.SaltEdge;

public class Signature
{
    public string Value { get; }

    public string KeyVersion { get; }

    private const string SignatureHeaderKey = "signature";

    private const string SignatureKeyVersionHeaderKey = "signature-key-version";

    public static Signature CreateFromHttpContext(HttpContext? context)
    {
        if (context is not null &&
            IsHeaderKeyAvailable(context, SignatureHeaderKey) &&
            IsHeaderKeyAvailable(context, SignatureKeyVersionHeaderKey))
        {
            var value = context.Request.Headers[SignatureHeaderKey].ToString();
            var keyVersion = context.Request.Headers[SignatureKeyVersionHeaderKey].ToString();

            return new Signature(value, keyVersion);
        }

        throw new ApplicationException("Http context and signature is not available");
    }

    private static bool IsHeaderKeyAvailable(HttpContext context, string headerKey)
    {
        return context.Request.Headers.Keys.Any(x => x == headerKey);
    }

    private Signature(string value, string keyVersion)
    {
        Value = value;
        KeyVersion = keyVersion;
    }
}
