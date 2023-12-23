using App.BuildingBlocks.Application;
using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Infrastructure.Data;
using App.BuildingBlocks.Tests.IntegrationTests.DependencyInjection;
using App.Modules.Categories.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace App.Modules.Categories.IntegrationTests.SeedWork;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("savify")
        .WithUsername("user")
        .WithPassword("pass")
        .Build();

    private string _saltEdgeMockServerUrl;

    public Task InitialiseDbContainerAsync() => _dbContainer.StartAsync();

    public string GetConnectionString() => _dbContainer.GetConnectionString();

    public static async Task<CustomWebApplicationFactory<TProgram>> Create(string saltEdgeMockServerUrl)
    {
        var factory = new CustomWebApplicationFactory<TProgram>(saltEdgeMockServerUrl);
        await factory.InitialiseDbContainerAsync();

        return factory;
    }

    public override async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseConfiguration(BuildConfiguration());

        builder.ConfigureTestServices(services =>
        {
            services.ReplaceDbContext<CategoriesContext>(_dbContainer.GetConnectionString());
            services.Replace(ServiceDescriptor.Scoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(_dbContainer.GetConnectionString())));
            services.Replace(ServiceDescriptor.Scoped<IExecutionContextAccessor>(_ => new ExecutionContextMock(Guid.NewGuid())));
        });
    }

    private IConfiguration BuildConfiguration()
    {
        var configurationValues = new List<KeyValuePair<string, string>>
        {
            new("SaltEdge:BaseUrl", _saltEdgeMockServerUrl)
        };

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Testing.json")
            .AddInMemoryCollection(configurationValues!)
            .Build();

        return configuration;
    }

    private CustomWebApplicationFactory(string saltEdgeMockServerUrl)
    {
        _saltEdgeMockServerUrl = saltEdgeMockServerUrl;
    }
}
