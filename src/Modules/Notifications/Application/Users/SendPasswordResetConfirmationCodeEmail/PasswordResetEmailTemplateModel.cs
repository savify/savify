using App.Modules.Notifications.Application.Emails.Templates;

namespace App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;

public class PasswordResetEmailTemplateModel(string name, string confirmationCode) : EmailTemplateModelBase
{
    public override string TemplateName => "password_reset";

    public string Name { get; } = name;

    public string ConfirmationCode { get; } = confirmationCode;
}
