using App.Modules.Notifications.Application.Emails.Templates;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationRenewalEmail;

public class RegistrationRenewalEmailTemplateModel(string name, string confirmationCode) : EmailTemplateModelBase
{
    public override string TemplateName => "registration_renewal";

    public string Name { get; } = name;

    public string ConfirmationCode { get; } = confirmationCode;
}
