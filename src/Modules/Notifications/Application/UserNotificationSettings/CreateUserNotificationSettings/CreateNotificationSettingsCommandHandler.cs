using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Domain.UserNotificationSettings;

namespace App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;

internal class CreateNotificationSettingsCommandHandler : ICommandHandler<CreateNotificationSettingsCommand>
{
    private readonly IUserNotificationSettingsRepository _userNotificationSettingsRepository;

    private readonly IUserNotificationSettingsCounter _userNotificationSettingsCounter;

    public CreateNotificationSettingsCommandHandler(
        IUserNotificationSettingsRepository userNotificationSettingsRepository,
        IUserNotificationSettingsCounter userNotificationSettingsCounter)
    {
        _userNotificationSettingsRepository = userNotificationSettingsRepository;
        _userNotificationSettingsCounter = userNotificationSettingsCounter;
    }

    public async Task Handle(CreateNotificationSettingsCommand command, CancellationToken cancellationToken)
    {
        var userNotificationSettings = Domain.UserNotificationSettings.UserNotificationSettings.Create(
            new UserId(command.UserId),
            command.Email,
            command.Name,
            Language.From(command.PreferredLanguage),
            _userNotificationSettingsCounter);

        await _userNotificationSettingsRepository.AddAsync(userNotificationSettings);
    }
}
