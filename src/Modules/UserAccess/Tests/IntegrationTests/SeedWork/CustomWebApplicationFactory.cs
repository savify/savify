using App.BuildingBlocks.Application;
using App.BuildingBlocks.Tests.IntegrationTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Modules.UserAccess.IntegrationTests.SeedWork;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Replace(ServiceDescriptor.Scoped<IExecutionContextAccessor>(_ => new ExecutionContextMock(Guid.NewGuid())));
        });
    }
}
