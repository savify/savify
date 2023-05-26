using App.BuildingBlocks.Application.Emails;
using App.BuildingBlocks.Application.Emails.Templates;
using App.BuildingBlocks.Infrastructure.Emails;
using App.Modules.UserAccess.Infrastructure.Emails;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Email;

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
