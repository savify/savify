using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Domain.UserNotificationSettings;

namespace App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;

internal class CreateNotificationSettingsCommandHandler(
    IUserNotificationSettingsRepository userNotificationSettingsRepository,
    IUserNotificationSettingsCounter userNotificationSettingsCounter)
    : ICommandHandler<CreateNotificationSettingsCommand>
{
    public async Task Handle(CreateNotificationSettingsCommand command, CancellationToken cancellationToken)
    {
        var userNotificationSettings = Domain.UserNotificationSettings.UserNotificationSettings.Create(
            new UserId(command.UserId),
            command.Email,
            command.Name,
            Language.From(command.PreferredLanguage),
            userNotificationSettingsCounter);

        await userNotificationSettingsRepository.AddAsync(userNotificationSettings);
    }
}
