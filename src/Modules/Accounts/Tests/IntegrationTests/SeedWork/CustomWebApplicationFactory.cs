using App.BuildingBlocks.Application;
using App.BuildingBlocks.Tests.IntegrationTests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Modules.Accounts.IntegrationTests.SeedWork;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public string ConnectionString { get; private set; }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        const string connectionStringEnvironmentVariable = "ASPNETCORE_INTEGRATION_TESTS_CONNECTION_STRING";
        ConnectionString = EnvironmentVariablesProvider.GetVariable(connectionStringEnvironmentVariable);
        
        if (ConnectionString == null)
        {
            throw new ApplicationException(
                $"Define connection string to integration tests database using environment variable: {connectionStringEnvironmentVariable}");
        }

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IExecutionContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor>(_ => new ExecutionContextMock(Guid.NewGuid()));
        });
    }
}
