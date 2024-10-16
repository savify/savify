using App.BuildingBlocks.Application;
using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Infrastructure.Data;
using App.BuildingBlocks.Tests.IntegrationTests.DependencyInjection;
using App.Modules.UserAccess.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace App.Modules.UserAccess.IntegrationTests.SeedWork;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly IExecutionContextAccessor _executionContextAccessor;

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("savify")
        .WithUsername("user")
        .WithPassword("pass")
        .Build();

    public Task InitialiseDbContainerAsync() => _dbContainer.StartAsync();

    public Task StopDbContainerAsync() => _dbContainer.StopAsync();

    public string GetConnectionString() => _dbContainer.GetConnectionString();

    public static async Task<CustomWebApplicationFactory<TProgram>> Create(IExecutionContextAccessor executionContextAccessor)
    {
        var factory = new CustomWebApplicationFactory<TProgram>(executionContextAccessor);
        await factory.InitialiseDbContainerAsync();

        return factory;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.ReplaceDbContext<UserAccessContext>(_dbContainer.GetConnectionString());
            services.Replace(ServiceDescriptor.Scoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(_dbContainer.GetConnectionString())));
            services.Replace(ServiceDescriptor.Scoped<IExecutionContextAccessor>(_ => _executionContextAccessor));
        });
    }

    private CustomWebApplicationFactory(IExecutionContextAccessor executionContextAccessor)
    {
        _executionContextAccessor = executionContextAccessor;
    }
}
