using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Application.Emails.Templates;
using App.Modules.Notifications.Infrastructure.Emails;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Notifications.Infrastructure.Configuration.DependencyInjection;

internal static class EmailingServiceCollectionExtensions
{
    internal static IServiceCollection AddEmailingServices(
        this IServiceCollection services,
        EmailConfiguration configuration,
        ILogger logger)
    {
        services.AddSingleton<EmailMessageMapper>(_ =>
        {
            return new EmailMessageMapper(configuration);
        });

        services.AddScoped<IEmailSender>(provider => new EmailSender(
            provider.GetRequiredService<EmailMessageMapper>(),
            configuration,
            logger));

        services.AddScoped<IEmailTemplateGenerator, EmailTemplateGenerator>();
        services.AddScoped<IEmailMessageFactory, EmailMessageFactory>();

        services.AddSingleton(configuration);

        return services;
    }
}
