using App.BuildingBlocks.Application;
using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Infrastructure.Data;
using App.Modules.Banks.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace App.Modules.Banks.IntegrationTests.SeedWork;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("savify")
        .WithUsername("user")
        .WithPassword("pass")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<BanksContext>));

            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<BanksContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
                options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
            });
            services.Replace(ServiceDescriptor.Scoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(_dbContainer.GetConnectionString())));
            services.Replace(ServiceDescriptor.Scoped<IExecutionContextAccessor>(_ => new ExecutionContextMock(Guid.NewGuid())));
        });
    }

    public Task InitialiseDbContainerAsync()
    {
        return _dbContainer.StartAsync();
    }

    public Task DisposeDbContainerAsync()
    {
        return _dbContainer.StopAsync();
    }

    public string GetConnectionString() => _dbContainer.GetConnectionString();
}
