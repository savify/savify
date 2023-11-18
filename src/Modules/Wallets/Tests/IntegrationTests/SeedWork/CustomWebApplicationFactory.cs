using App.BuildingBlocks.Application;
using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.Wallets.Infrastructure;
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

            // TODO: find some solution to work with domain notifications maps without duplication in tests!
            var domainNotificationsMap = new BiDictionary<string, Type>();

            // domainNotificationsMap.Add(nameof(ExampleDomainEvent), typeof(ExampleNotification));

            services.Replace(ServiceDescriptor.Scoped<IDomainNotificationsMapper<WalletsContext>>(_ => new DomainNotificationsMapper<WalletsContext>(domainNotificationsMap)));
        });
    }
}
