using App.Modules.Notifications.Application.Emails.Templates;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;

public class RegistrationConfirmationEmailTemplateModel : EmailTemplateModelBase
{
    public override string TemplateName => "registration_confirmation";

    public string Name { get; }

    public string ConfirmationCode { get; }

    public RegistrationConfirmationEmailTemplateModel(string name, string confirmationCode)
    {
        Name = name;
        ConfirmationCode = confirmationCode;
    }
}
