using App.BuildingBlocks.Application;
using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.UserAccess.Infrastructure;
using App.Modules.Wallets.Infrastructure;
using App.Modules.Wallets.Infrastructure.Outbox;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Modules.Wallets.IntegrationTests.SeedWork;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Replace(ServiceDescriptor.Scoped<IExecutionContextAccessor>(_ => new ExecutionContextMock(Guid.NewGuid())));

            services.Replace(ServiceDescriptor.Scoped<IUnitOfWork>(provider => new UnitOfWork(
                provider.GetRequiredService<WalletsContext>(),
                provider.GetRequiredService<IDomainEventsDispatcher>())));

            services.Replace(ServiceDescriptor.Scoped<IDomainEventsAccessor>(provider => new DomainEventsAccessor(
                provider.GetRequiredService<UserAccessContext>())));

            // TODO: find some solution to work with domain notifications maps without duplication in tests!
            var domainNotificationsMap = new BiDictionary<string, Type>();

            // domainNotificationsMap.Add(nameof(ExampleDomainEvent), typeof(ExampleNotification));

            services.Replace(ServiceDescriptor.Scoped<IDomainNotificationsMapper>(_ => new DomainNotificationsMapper(domainNotificationsMap)));
            services.Replace(ServiceDescriptor.Scoped<IOutbox>(provider => new Outbox(provider.GetRequiredService<WalletsContext>())));
        });
    }
}
