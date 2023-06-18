using App.BuildingBlocks.Application;
using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Modules.Notifications.IntegrationTests.SeedWork;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private IEmailSender _emailSender;

    public CustomWebApplicationFactory(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Replace(ServiceDescriptor.Scoped<IExecutionContextAccessor>(_ => new ExecutionContextMock(Guid.NewGuid())));
            services.Replace(ServiceDescriptor.Scoped<IEmailSender>(_ => _emailSender));
            services.Replace(ServiceDescriptor.Scoped<IUnitOfWork>(provider => new UnitOfWork(
                provider.GetRequiredService<NotificationsContext>(),
                provider.GetRequiredService<IDomainEventsDispatcher>())));
        });
    }
}
