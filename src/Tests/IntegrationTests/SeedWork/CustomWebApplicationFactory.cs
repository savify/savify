using App.BuildingBlocks.Application;
using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Infrastructure.Data;
using App.BuildingBlocks.Tests.IntegrationTests.DependencyInjection;
using App.Modules.Banks.Infrastructure;
using App.Modules.Categories.Infrastructure;
using App.Modules.FinanceTracking.Infrastructure;
using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Infrastructure;
using App.Modules.UserAccess.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace App.IntegrationTests.SeedWork;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private IEmailSender _emailSender;

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("savify")
        .WithUsername("user")
        .WithPassword("pass")
        .Build();

    public static async Task<CustomWebApplicationFactory<TProgram>> Create(IEmailSender emailSender)
    {
        var factory = new CustomWebApplicationFactory<TProgram>(emailSender);
        await factory.InitialiseDbContainerAsync();

        return factory;
    }

    public Task InitialiseDbContainerAsync() => _dbContainer.StartAsync();

    public Task StopDbContainerAsync() => _dbContainer.StopAsync();

    public string GetConnectionString() => _dbContainer.GetConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var connectionString = _dbContainer.GetConnectionString();

            services.ReplaceDbContext<BanksContext>(connectionString);
            services.ReplaceDbContext<CategoriesContext>(connectionString);
            services.ReplaceDbContext<FinanceTrackingContext>(connectionString);
            services.ReplaceDbContext<NotificationsContext>(connectionString);
            services.ReplaceDbContext<UserAccessContext>(connectionString);

            services.Replace(ServiceDescriptor.Scoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(_dbContainer.GetConnectionString())));
            services.Replace(ServiceDescriptor.Transient<IExecutionContextAccessor>(_ => new ExecutionContextMock(Guid.NewGuid())));
            services.Replace(ServiceDescriptor.Scoped<IEmailSender>(_ => _emailSender));
        });
    }

    private CustomWebApplicationFactory(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }
}
