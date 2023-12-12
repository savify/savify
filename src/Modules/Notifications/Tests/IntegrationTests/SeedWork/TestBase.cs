using System.Data;
using App.API;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.Database.Scripts.Clear;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Infrastructure;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NSubstitute;

namespace App.Modules.Notifications.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected INotificationsModule NotificationsModule { get; private set; }

    protected string ConnectionString { get; private set; }

    protected IEmailSender EmailSender { get; private set; }

    [OneTimeSetUp]
    public async Task Init()
    {
        EmailSender = Substitute.For<IEmailSender>();
        WebApplicationFactory = new CustomWebApplicationFactory<Program>(EmailSender);
        CompositionRoot.SetServiceProvider(WebApplicationFactory.Services);

        await WebApplicationFactory.InitialiseDbContainerAsync();
        ConnectionString = WebApplicationFactory.GetConnectionString();

        using var scope = WebApplicationFactory.Services.CreateScope();
        NotificationsModule = scope.ServiceProvider.GetRequiredService<INotificationsModule>();

        var dbContext = scope.ServiceProvider.GetRequiredService<NotificationsContext>();
        await dbContext.Database.MigrateAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await WebApplicationFactory.DisposeDbContainerAsync();
        await WebApplicationFactory.DisposeAsync();
    }

    [SetUp]
    public async Task BeforeEachTest()
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);
        await ClearDatabase(sqlConnection);
    }

    private static async Task ClearDatabase(IDbConnection connection)
    {
        var sql = await ClearDatabaseCommandProvider.GetAsync();

        await connection.ExecuteScalarAsync(sql);
    }
}
