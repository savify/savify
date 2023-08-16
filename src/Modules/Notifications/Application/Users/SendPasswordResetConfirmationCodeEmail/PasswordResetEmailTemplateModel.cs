using App.Modules.Notifications.Application.Emails.Templates;

namespace App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;

public class PasswordResetEmailTemplateModel : EmailTemplateModelBase
{
    public override string TemplateName => "password_reset";

    public string Name { get; }

    public string ConfirmationCode { get; }

    public PasswordResetEmailTemplateModel(string name, string confirmationCode)
    {
        Name = name;
        ConfirmationCode = confirmationCode;
    }
}
