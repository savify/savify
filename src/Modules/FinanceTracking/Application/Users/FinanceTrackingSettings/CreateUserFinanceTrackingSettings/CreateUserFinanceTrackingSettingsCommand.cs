using App.Modules.FinanceTracking.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace App.Modules.FinanceTracking.Application.Users.FinanceTrackingSettings.CreateUserFinanceTrackingSettings;

[method: JsonConstructor]
public class CreateUserFinanceTrackingSettingsCommand(
    Guid id,
    Guid correlationId,
    Guid userId,
    string countryCode,
    string preferredLanguage)
    : InternalCommandBase(id, correlationId)
{
    internal Guid UserId { get; } = userId;

    internal string CountryCode { get; } = countryCode;

    internal string PreferredLanguage { get; } = preferredLanguage;
}
