using App.Modules.Notifications.Application.Emails.Templates;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationRenewalEmail;

public class RegistrationRenewalEmailTemplateModel : EmailTemplateModelBase
{
    public override string TemplateName => "registration_renewal";

    public string Name { get; }

    public string ConfirmationCode { get; }

    public RegistrationRenewalEmailTemplateModel(string name, string confirmationCode)
    {
        Name = name;
        ConfirmationCode = confirmationCode;
    }
}
