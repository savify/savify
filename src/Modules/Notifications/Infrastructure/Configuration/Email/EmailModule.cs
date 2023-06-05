using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Application.Emails.Templates;
using App.Modules.Notifications.Infrastructure.Emails;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Notifications.Infrastructure.Configuration.Email;

internal static class EmailModule
{
    internal static void Configure(
        IServiceCollection services,
        EmailConfiguration configuration,
        IEmailSender? emailSender = null,
        IEmailMessageFactory? emailMessageFactory = null)
    {
        if (emailSender != null && emailMessageFactory != null)
        {
            services.AddSingleton(emailSender);
            services.AddSingleton(emailMessageFactory);
        }
        else
        {
            services.AddSingleton<EmailMessageMapper>(_ =>
            {
                return new EmailMessageMapper(configuration);
            });
            
            services.AddScoped<IEmailSender>(provider =>
            {
                return new EmailSender(
                    provider.GetService<EmailMessageMapper>(),
                    configuration,
                    provider.GetService<ILogger>());
            });

            services.AddScoped<IEmailTemplateGenerator, EmailTemplateGenerator>();
            services.AddScoped<IEmailMessageFactory, EmailMessageFactory>();
        }

        services.AddSingleton(configuration);
    }
}
