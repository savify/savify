using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;

public class CreateNotificationSettingsCommand : InternalCommandBase<Result>
{
    [JsonConstructor]
    public CreateNotificationSettingsCommand(Guid id, Guid userId, string name, string email, string preferredLanguage) : base(id)
    {
        UserId = userId;
        Name = name;
        Email = email;
        PreferredLanguage = preferredLanguage;
    }

    internal Guid UserId { get; }
    
    internal string Name { get; }

    internal string Email { get; }
    
    internal string PreferredLanguage { get; }
}
