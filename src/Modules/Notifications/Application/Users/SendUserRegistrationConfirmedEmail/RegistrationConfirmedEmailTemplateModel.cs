using App.Modules.Notifications.Application.Emails.Templates;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;

public class RegistrationConfirmedEmailTemplateModel : EmailTemplateModelBase
{
    public override string TemplateName => "registration_confirmed";
    
    public string Name { get; }

    public RegistrationConfirmedEmailTemplateModel(string name)
    {
        Name = name;
    }
}
