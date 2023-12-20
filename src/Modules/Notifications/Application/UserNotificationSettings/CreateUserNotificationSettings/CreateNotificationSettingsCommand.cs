using App.Modules.Notifications.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;

[method: JsonConstructor]
public class CreateNotificationSettingsCommand(
    Guid id,
    Guid correlationId,
    Guid userId,
    string name,
    string email,
    string preferredLanguage)
    : InternalCommandBase(id, correlationId)
{
    internal Guid UserId { get; } = userId;

    internal string Name { get; } = name;

    internal string Email { get; } = email;

    internal string PreferredLanguage { get; } = preferredLanguage;
}
