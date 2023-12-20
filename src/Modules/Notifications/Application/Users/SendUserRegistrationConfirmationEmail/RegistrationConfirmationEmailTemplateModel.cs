using App.Modules.Notifications.Application.Emails.Templates;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;

public class RegistrationConfirmationEmailTemplateModel(string name, string confirmationCode) : EmailTemplateModelBase
{
    public override string TemplateName => "registration_confirmation";

    public string Name { get; } = name;

    public string ConfirmationCode { get; } = confirmationCode;
}
